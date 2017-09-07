#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include <detours.h>
#include "DetectPlayerId.h"
#include "Globals.h"

HANDLE DetectPlayerId::m_hEvent;
DataQueue* DetectPlayerId::m_dataQueue;
DetectPlayerId::OriginalMethodPtr DetectPlayerId::originalMethod;
DetectPlayerId::GetObjectIdMethodPtr DetectPlayerId::GetObjectId;

void DetectPlayerId::EnableHook() {
	GetObjectId = (GetObjectIdMethodPtr)GetProcAddress(::GetModuleHandle("Engine.dll"), "?GetObjectId@Object@GAME@@QBEIXZ");
	originalMethod = (OriginalMethodPtr)GetProcAddress(::GetModuleHandle("Game.dll"), "??0Player@GAME@@QAE@XZ");
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach((PVOID*)&originalMethod, HookedMethod);
	DetourTransactionCommit();
}

DetectPlayerId::DetectPlayerId(DataQueue* dataQueue, HANDLE hEvent) {
	DetectPlayerId::m_dataQueue = dataQueue;
	DetectPlayerId::m_hEvent = hEvent;
}

DetectPlayerId::DetectPlayerId() {
	DetectPlayerId::m_hEvent = NULL;
}

void DetectPlayerId::DisableHook() {
	LONG res1 = DetourTransactionBegin();
	LONG res2 = DetourUpdateThread(GetCurrentThread());
	DetourDetach((PVOID*)&originalMethod, HookedMethod);
	DetourTransactionCommit();
}

void* __fastcall DetectPlayerId::HookedMethod(void* This, void* notUsed) {
	void* v = originalMethod(This);

	int id = GetObjectId(This);
	DataItemPtr item(new DataItem(TYPE_DetectPlayerId, sizeof(id), (char*)&id));
	m_dataQueue->push(item);
	SetEvent(m_hEvent);

	return v;
}
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
	originalMethod = (OriginalMethodPtr)GetProcAddress(::GetModuleHandle("Game.dll"), "?SetMainPlayer@Player@GAME@@QAEX_N@Z");
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

void* __fastcall DetectPlayerId::HookedMethod(void* This, void* notUsed, bool b) {
	void* v = originalMethod(This, b);

	const size_t resultSize = sizeof(int) + sizeof(bool);
	char result[resultSize] = { 0 };
	result[0] = b ? 1 : 0;

	int id = GetObjectId((void*)This);
	memcpy(result + 1, &id, sizeof(id));

	DataItemPtr item(new DataItem(TYPE_DetectPlayerId, 5, (char*)&result));
	m_dataQueue->push(item);
	SetEvent(m_hEvent);


	return v;
}
#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include <detours.h>
#include "PauseGameTime.h"
#include "Globals.h"

HANDLE PauseGameTime::m_hEvent;
DataQueue* PauseGameTime::m_dataQueue;
PauseGameTime::OriginalMethodPtr PauseGameTime::originalMethod;

void PauseGameTime::EnableHook() {
	originalMethod = (OriginalMethodPtr)GetProcAddress(::GetModuleHandle("Engine.dll"), "?PauseGameTime@GAME@@YAXXZ");
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach((PVOID*)&originalMethod, HookedMethod);
	DetourTransactionCommit();
}

PauseGameTime::PauseGameTime(DataQueue* dataQueue, HANDLE hEvent) {
	PauseGameTime::m_dataQueue = dataQueue;
	PauseGameTime::m_hEvent = hEvent;
}

PauseGameTime::PauseGameTime() {
	PauseGameTime::m_hEvent = NULL;
}

void PauseGameTime::DisableHook() {
	LONG res1 = DetourTransactionBegin();
	LONG res2 = DetourUpdateThread(GetCurrentThread());
	DetourDetach((PVOID*)&originalMethod, HookedMethod);
	DetourTransactionCommit();
}

void* __fastcall PauseGameTime::HookedMethod(void* This, void* notUsed) {
	DataItemPtr item(new DataItem(TYPE_PauseGameTime, 0, 0));
	m_dataQueue->push(item);
	SetEvent(m_hEvent);

	void* v = originalMethod(This);
	return v;
}
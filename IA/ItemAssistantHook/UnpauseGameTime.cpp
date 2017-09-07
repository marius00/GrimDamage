#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include <detours.h>
#include "UnpauseGameTime.h"
#include "Globals.h"

HANDLE UnpauseGameTime::m_hEvent;
DataQueue* UnpauseGameTime::m_dataQueue;
UnpauseGameTime::OriginalMethodPtr UnpauseGameTime::originalMethod;

void UnpauseGameTime::EnableHook() {
	originalMethod = (OriginalMethodPtr)GetProcAddress(::GetModuleHandle("Engine.dll"), "?UnpauseGameTime@GAME@@YAXXZ");
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach((PVOID*)&originalMethod, HookedMethod);
	DetourTransactionCommit();
}

UnpauseGameTime::UnpauseGameTime(DataQueue* dataQueue, HANDLE hEvent) {
	UnpauseGameTime::m_dataQueue = dataQueue;
	UnpauseGameTime::m_hEvent = hEvent;
}

UnpauseGameTime::UnpauseGameTime() {
	UnpauseGameTime::m_hEvent = NULL;
}

void UnpauseGameTime::DisableHook() {
	LONG res1 = DetourTransactionBegin();
	LONG res2 = DetourUpdateThread(GetCurrentThread());
	DetourDetach((PVOID*)&originalMethod, HookedMethod);
	DetourTransactionCommit();
}

void* __fastcall UnpauseGameTime::HookedMethod(void* This, void* notUsed) {
	DataItemPtr item(new DataItem(TYPE_UnpauseGameTime, 0, 0));
	m_dataQueue->push(item);
	SetEvent(m_hEvent);

	void* v = originalMethod(This);
	return v;
}
#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include <detours.h>
#include "IncrementDeaths.h"
#include "Globals.h"

HANDLE IncrementDeaths::m_hEvent;
DataQueue* IncrementDeaths::m_dataQueue;
IncrementDeaths::OriginalMethodPtr IncrementDeaths::originalMethod;

void IncrementDeaths::EnableHook() {
	originalMethod = (OriginalMethodPtr)GetProcAddress(::GetModuleHandle("Game.dll"), "?DeathUpdate@ControllerPlayer@GAME@@UAEXH@Z");
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach((PVOID*)&originalMethod, HookedMethod);
	DetourTransactionCommit();
}

IncrementDeaths::IncrementDeaths(DataQueue* dataQueue, HANDLE hEvent) {
	IncrementDeaths::m_dataQueue = dataQueue;
	IncrementDeaths::m_hEvent = hEvent;
}

IncrementDeaths::IncrementDeaths() {
	IncrementDeaths::m_hEvent = NULL;
}

void IncrementDeaths::DisableHook() {
	LONG res1 = DetourTransactionBegin();
	LONG res2 = DetourUpdateThread(GetCurrentThread());
	DetourDetach((PVOID*)&originalMethod, HookedMethod);
	DetourTransactionCommit();
}

void* __fastcall IncrementDeaths::HookedMethod(void* This, void* notUsed, int val) {
	DataItemPtr item(new DataItem(TYPE_IncrementDeaths, 0, 0));
	m_dataQueue->push(item);
	SetEvent(m_hEvent);

	void* v = originalMethod(This, val);
	return v;
}
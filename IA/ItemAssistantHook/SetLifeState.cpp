#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include <detours.h>
#include "SetLifeState.h"
#include "Globals.h"

HANDLE SetLifeState::m_hEvent;
DataQueue* SetLifeState::m_dataQueue;
SetLifeState::OriginalMethodPtr SetLifeState::originalMethod;

void SetLifeState::EnableHook() {
	originalMethod = (OriginalMethodPtr)GetProcAddress(::GetModuleHandle("Game.dll"), "?SetLifeState@Character@GAME@@UAEXW4Character_LifeState@2@@Z");
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach((PVOID*)&originalMethod, HookedMethod);
	DetourTransactionCommit();
}

SetLifeState::SetLifeState(DataQueue* dataQueue, HANDLE hEvent) {
	SetLifeState::m_dataQueue = dataQueue;
	SetLifeState::m_hEvent = hEvent;
}

SetLifeState::SetLifeState() {
	SetLifeState::m_hEvent = NULL;
}

void SetLifeState::DisableHook() {
	LONG res1 = DetourTransactionBegin();
	LONG res2 = DetourUpdateThread(GetCurrentThread());
	DetourDetach((PVOID*)&originalMethod, HookedMethod);
	DetourTransactionCommit();
}

void* __fastcall SetLifeState::HookedMethod(void* This, void* notUsed, int state) {
	DataItemPtr item(new DataItem(TYPE_SetLifeState, sizeof(state), (char*)&state));
	m_dataQueue->push(item);
	SetEvent(m_hEvent);

	void* v = originalMethod(This, state);
	return v;
}
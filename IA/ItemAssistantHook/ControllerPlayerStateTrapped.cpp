#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include <detours.h>
#include "ControllerPlayerStateTrapped.h"
#include "Globals.h"

HANDLE ControllerPlayerStateTrapped::m_hEvent;
DataQueue* ControllerPlayerStateTrapped::m_dataQueue;
ControllerPlayerStateTrapped::MethodPtr ControllerPlayerStateTrapped::originalBegin;
ControllerPlayerStateTrapped::MethodPtr ControllerPlayerStateTrapped::originalEnd;

void ControllerPlayerStateTrapped::EnableHook() {
	originalBegin = (MethodPtr)GetProcAddress(::GetModuleHandle("Game.dll"), "?OnBegin@ControllerPlayerStateTrapped@GAME@@UAEXXZ");
	originalEnd = (MethodPtr)GetProcAddress(::GetModuleHandle("Game.dll"), "?OnEnd@ControllerPlayerStateTrapped@GAME@@UAEXXZ");
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach((PVOID*)&originalBegin, HookedBegin);
	DetourAttach((PVOID*)&originalEnd, HookedEnd);
	DetourTransactionCommit();
}

ControllerPlayerStateTrapped::ControllerPlayerStateTrapped(DataQueue* dataQueue, HANDLE hEvent) {
	ControllerPlayerStateTrapped::m_dataQueue = dataQueue;
	ControllerPlayerStateTrapped::m_hEvent = hEvent;
}

ControllerPlayerStateTrapped::ControllerPlayerStateTrapped() {
	ControllerPlayerStateTrapped::m_hEvent = NULL;
}

void ControllerPlayerStateTrapped::DisableHook() {
	LONG res1 = DetourTransactionBegin();
	LONG res2 = DetourUpdateThread(GetCurrentThread());
	DetourDetach((PVOID*)&originalBegin, HookedBegin);
	DetourDetach((PVOID*)&originalEnd, HookedEnd);
	DetourTransactionCommit();
}

void* __fastcall ControllerPlayerStateTrapped::HookedBegin(void* This, void* notUsed) {
	DataItemPtr item(new DataItem(TYPE_TrapBegin, 0, 0));
	m_dataQueue->push(item);
	SetEvent(m_hEvent);

	void* v = originalBegin(This);
	return v;
}
void* __fastcall ControllerPlayerStateTrapped::HookedEnd(void* This, void* notUsed) {
	DataItemPtr item(new DataItem(TYPE_TrapEnd, 0, 0));
	m_dataQueue->push(item);
	SetEvent(m_hEvent);

	void* v = originalEnd(This);
	return v;
}
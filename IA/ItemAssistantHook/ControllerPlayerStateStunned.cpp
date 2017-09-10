#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include <detours.h>
#include "ControllerPlayerStateStunned.h"
#include "Globals.h"

HANDLE ControllerPlayerStateStunned::m_hEvent;
DataQueue* ControllerPlayerStateStunned::m_dataQueue;
ControllerPlayerStateStunned::MethodPtr ControllerPlayerStateStunned::originalBegin;
ControllerPlayerStateStunned::MethodPtr ControllerPlayerStateStunned::originalEnd;

void ControllerPlayerStateStunned::EnableHook() {
	originalBegin = (MethodPtr)GetProcAddress(::GetModuleHandle("Game.dll"), "?OnBegin@ControllerPlayerStateStunned@GAME@@UAEXXZ");
	originalEnd = (MethodPtr)GetProcAddress(::GetModuleHandle("Game.dll"), "?OnEnd@ControllerPlayerStateStunned@GAME@@UAEXXZ");
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach((PVOID*)&originalBegin, HookedBegin);
	DetourAttach((PVOID*)&originalEnd, HookedEnd);
	DetourTransactionCommit();
}

ControllerPlayerStateStunned::ControllerPlayerStateStunned(DataQueue* dataQueue, HANDLE hEvent) {
	ControllerPlayerStateStunned::m_dataQueue = dataQueue;
	ControllerPlayerStateStunned::m_hEvent = hEvent;
}

ControllerPlayerStateStunned::ControllerPlayerStateStunned() {
	ControllerPlayerStateStunned::m_hEvent = NULL;
}

void ControllerPlayerStateStunned::DisableHook() {
	LONG res1 = DetourTransactionBegin();
	LONG res2 = DetourUpdateThread(GetCurrentThread());
	DetourDetach((PVOID*)&originalBegin, HookedBegin);
	DetourDetach((PVOID*)&originalEnd, HookedEnd);
	DetourTransactionCommit();
}

void* __fastcall ControllerPlayerStateStunned::HookedBegin(void* This, void* notUsed) {
	DataItemPtr item(new DataItem(TYPE_StunBegin, 0, 0));
	m_dataQueue->push(item);
	SetEvent(m_hEvent);

	void* v = originalBegin(This);
	return v;
}
void* __fastcall ControllerPlayerStateStunned::HookedEnd(void* This, void* notUsed) {
	DataItemPtr item(new DataItem(TYPE_StunEnd, 0, 0));
	m_dataQueue->push(item);
	SetEvent(m_hEvent);

	void* v = originalEnd(This);
	return v;
}
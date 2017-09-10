#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include <detours.h>
#include "DisableMovement.h"
#include "Globals.h"

HANDLE DisableMovement::m_hEvent;
DataQueue* DisableMovement::m_dataQueue;
DisableMovement::OriginalMethodPtr DisableMovement::originalMethod;

void DisableMovement::EnableHook() {
	originalMethod = (OriginalMethodPtr)GetProcAddress(::GetModuleHandle("Engine.dll"), "?DisableMovement@Character@GAME@@QAEXXZ");
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach((PVOID*)&originalMethod, HookedMethod);
	DetourTransactionCommit();
}

DisableMovement::DisableMovement(DataQueue* dataQueue, HANDLE hEvent) {
	DisableMovement::m_dataQueue = dataQueue;
	DisableMovement::m_hEvent = hEvent;
}

DisableMovement::DisableMovement() {
	DisableMovement::m_hEvent = NULL;
}

void DisableMovement::DisableHook() {
	LONG res1 = DetourTransactionBegin();
	LONG res2 = DetourUpdateThread(GetCurrentThread());
	DetourDetach((PVOID*)&originalMethod, HookedMethod);
	DetourTransactionCommit();
}

void* __fastcall DisableMovement::HookedMethod(void* This, void* notUsed) {
	DataItemPtr item(new DataItem(TYPE_DisableMovement, 0, 0));
	m_dataQueue->push(item);
	SetEvent(m_hEvent);

	void* v = originalMethod(This);
	return v;
}
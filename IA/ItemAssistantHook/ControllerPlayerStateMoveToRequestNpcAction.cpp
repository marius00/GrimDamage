#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include <detours.h>
#include "ControllerPlayerStateMoveToRequestNpcAction.h"
#include "Globals.h"

HANDLE ControllerPlayerStateMoveToRequestNpcAction::m_hEvent;
DataQueue* ControllerPlayerStateMoveToRequestNpcAction::m_dataQueue;
ControllerPlayerStateMoveToRequestNpcAction::OriginalMethodPtr ControllerPlayerStateMoveToRequestNpcAction::originalMethod;

void ControllerPlayerStateMoveToRequestNpcAction::EnableHook() {
	originalMethod = (OriginalMethodPtr)GetProcAddress(::GetModuleHandle("Game.dll"), "?RequestNpcAction@ControllerPlayerStateMoveTo@GAME@@MAEX_N0ABVWorldVec3@2@PBVNpc@2@@Z");
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach((PVOID*)&originalMethod, HookedMethod);
	DetourTransactionCommit();
}

ControllerPlayerStateMoveToRequestNpcAction::ControllerPlayerStateMoveToRequestNpcAction(DataQueue* dataQueue, HANDLE hEvent) {
	ControllerPlayerStateMoveToRequestNpcAction::m_dataQueue = dataQueue;
	ControllerPlayerStateMoveToRequestNpcAction::m_hEvent = hEvent;
}

ControllerPlayerStateMoveToRequestNpcAction::ControllerPlayerStateMoveToRequestNpcAction() {
	ControllerPlayerStateMoveToRequestNpcAction::m_hEvent = NULL;
}

void ControllerPlayerStateMoveToRequestNpcAction::DisableHook() {
	LONG res1 = DetourTransactionBegin();
	LONG res2 = DetourUpdateThread(GetCurrentThread());
	DetourDetach((PVOID*)&originalMethod, HookedMethod);
	DetourTransactionCommit();
}

void* __fastcall ControllerPlayerStateMoveToRequestNpcAction::HookedMethod(void* This, void* notUsed, bool a, bool b, Vec3f const & xyz, void* npc) {

	const size_t bufflen = sizeof(Vec3f) + sizeof(int) * 4;
	char buffer[bufflen];

	size_t pos = 0;

	memcpy(buffer + pos, &xyz, sizeof(Vec3f));
	pos += sizeof(Vec3f);
	

	int* ptr = (int*)&xyz;
	char* regionId = (char*)ptr[0] + 6 * 4;

	SIZE_T bytesRead = 0;
	HANDLE hProcess = GetCurrentProcess();
	if (ReadProcessMemory(hProcess, (void*)regionId, (char*)&buffer + pos, 16, &bytesRead) != 0) {
		DataItemPtr item(new DataItem(TYPE_ControllerPlayerStateMoveToRequestNpcAction, bufflen, (char*)buffer));
		m_dataQueue->push(item);
		SetEvent(m_hEvent);
	}

	void* v = originalMethod(This, a, b, xyz, npc);
	return v;
}
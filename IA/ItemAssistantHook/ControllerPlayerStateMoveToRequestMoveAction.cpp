//	HookGame("?RequestMoveAction@ControllerPlayerStateMoveTo@GAME@@MAEX_N0ABVWorldVec3@2@@Z", HookedMethod, m_dataQueue, m_hEvent, TYPE_ControllerPlayerStateMoveToRequestMoveAction);
#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include <detours.h>
#include "ControllerPlayerStateMoveToRequestMoveAction.h"
#include "Globals.h"

HANDLE ControllerPlayerStateMoveToRequestMoveAction::m_hEvent;
DataQueue* ControllerPlayerStateMoveToRequestMoveAction::m_dataQueue;
ControllerPlayerStateMoveToRequestMoveAction::OriginalMethodPtr ControllerPlayerStateMoveToRequestMoveAction::originalMethod;

void ControllerPlayerStateMoveToRequestMoveAction::EnableHook() {
	originalMethod = (OriginalMethodPtr)GetProcAddress(::GetModuleHandle("Game.dll"), "?RequestMoveAction@ControllerPlayerStateMoveTo@GAME@@MAEX_N0ABVWorldVec3@2@@Z");
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach((PVOID*)&originalMethod, HookedMethod);
	DetourTransactionCommit();

	HookGame("?RequestMoveAction@ControllerPlayerStateMoveTo@GAME@@MAEX_N0ABVWorldVec3@2@@Z", HookedMethod, m_dataQueue, m_hEvent, TYPE_ControllerPlayerStateMoveToRequestMoveAction);
}

ControllerPlayerStateMoveToRequestMoveAction::ControllerPlayerStateMoveToRequestMoveAction(DataQueue* dataQueue, HANDLE hEvent) {
	ControllerPlayerStateMoveToRequestMoveAction::m_dataQueue = dataQueue;
	ControllerPlayerStateMoveToRequestMoveAction::m_hEvent = hEvent;
}

ControllerPlayerStateMoveToRequestMoveAction::ControllerPlayerStateMoveToRequestMoveAction() {
	ControllerPlayerStateMoveToRequestMoveAction::m_hEvent = NULL;
}

void ControllerPlayerStateMoveToRequestMoveAction::DisableHook() {
	LONG res1 = DetourTransactionBegin();
	LONG res2 = DetourUpdateThread(GetCurrentThread());
	DetourDetach((PVOID*)&originalMethod, HookedMethod);
	DetourTransactionCommit();
}

void* __fastcall ControllerPlayerStateMoveToRequestMoveAction::HookedMethod(void* This, void* notUsed, bool a, bool b, Vec3f const & xyz) {

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
		DataItemPtr item(new DataItem(TYPE_ControllerPlayerStateMoveToRequestMoveAction, bufflen, (char*)buffer));
		m_dataQueue->push(item);
		SetEvent(m_hEvent);
	}

	void* v = originalMethod(This, a, b, xyz);
	return v;

}
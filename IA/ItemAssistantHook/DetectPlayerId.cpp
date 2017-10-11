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
int DetectPlayerId::m_offset = 0;

bool DetectPlayerId::DetectOffset() {
	SIZE_T bytesRead = 0;
	HANDLE hProcess = GetCurrentProcess();
	const size_t resultSize = 31;
	unsigned char result[resultSize] = { 0 };

	char* addr = (char*)GetProcAddress(::GetModuleHandle("Game.dll"), "?SetMainPlayer@Player@GAME@@QAEX_N@Z");

	int offset = 0;
	if (ReadProcessMemory(hProcess, addr, (void*)&result, 30, &bytesRead) != 0) {
		for (int i = 0; i < bytesRead - 6; i++) {
			if ((int)result[i] == 0x88 && (int)result[i + 1] == 0x81) {
				
				memcpy(&offset, &result[i + 2], 4);
			
				DataItemPtr item(new DataItem(TYPE_PlayerPrimaryIDOffsetDetected, sizeof(offset), (char*)&offset));
				m_dataQueue->push(item);
				SetEvent(m_hEvent);

				m_offset = offset;
				return true;
			}
		}
	}

	DataItemPtr item(new DataItem(TYPE_ErrorDetectingPrimaryPlayerIdOffset, resultSize, (char*)result));
	m_dataQueue->push(item);
	SetEvent(m_hEvent);
	return false;
}

int DetectPlayerId::GetPlayerId(int* player) {
	if (m_offset == 0)
		return -1;

	SIZE_T bytesRead = 0;
	HANDLE hProcess = GetCurrentProcess();
	unsigned int playerId = 0;

	if (ReadProcessMemory(hProcess, player + m_offset, (void*)&playerId, 4, &bytesRead) != 0) {
		return playerId;
	}
	
	return -1;
}

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
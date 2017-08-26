#include "CharacterSubtractLife.h"
#include <stdio.h>
#include <stdlib.h>
#include "../MessageType.h"
#include <detours.h>
#include "CharacterSubtractLife.h"
#include "../MessageType.h"

HANDLE CharacterSubtractLife::m_hEvent;
DataQueue* CharacterSubtractLife::m_dataQueue;
CharacterSubtractLife::OriginalMethodPtr CharacterSubtractLife::originalMethod;

void* __fastcall HookedMethod(void* This, void* _, float f, int* PlayStatsDamageType, bool a, bool b) {
	const int buffsize = sizeof(float) + sizeof(int) + sizeof(bool) + sizeof(bool) + sizeof(int) * 2;
	char buffer[buffsize] = { 0 };

	int ptr = 0;
	memcpy(&buffer + ptr, (char*)&f, sizeof(float));
	ptr += sizeof(float);

	int dmg = (PlayStatsDamageType == NULL) ? (int)PlayStatsDamageType : *((int*)PlayStatsDamageType);
	memcpy(&buffer + ptr, (char*)&dmg, sizeof(int));
	ptr += sizeof(int);

	memcpy(&buffer + ptr, (char*)&a, sizeof(bool));
	ptr += sizeof(bool);

	memcpy(&buffer + ptr, (char*)&b, sizeof(bool));
	ptr += sizeof(bool);

	int poop = -1;
	__asm {
		push eax
		mov poop, ecx
		pop eax
	}

	int _this = This == NULL ? (int)This : *((int*)This);
	memcpy(&buffer + ptr, (char*)&_this, sizeof(int));
	ptr += sizeof(int);

	int ___ = (int)poop;
	memcpy(&buffer + ptr, (char*)&___, sizeof(int));
	ptr += sizeof(int);

	DataItemPtr item(new DataItem(Type_CharacterSubtractLife, buffsize, (char*)&buffer));
	CharacterSubtractLife::m_dataQueue->push(item);
	SetEvent(CharacterSubtractLife::m_hEvent);

	void* v = CharacterSubtractLife::originalMethod(This, f, PlayStatsDamageType, a, b);
	return v;
}

void CharacterSubtractLife::EnableHook() {

	originalMethod = (OriginalMethodPtr)GetProcAddress(::GetModuleHandle("Game.dll"), "?SubtractLife@Character@GAME@@QAEXMABUPlayStatsDamageType@2@_N_N@Z");
	if (originalMethod == NULL) {
		DataItemPtr item(new DataItem(Type_CharacterSubtractLife, 0, NULL));
		m_dataQueue->push(item);
		SetEvent(m_hEvent);
	}

	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach((PVOID*)&originalMethod, HookedMethod);
	DetourTransactionCommit();
}

CharacterSubtractLife::CharacterSubtractLife(DataQueue* dataQueue, HANDLE hEvent) {
	m_dataQueue = dataQueue;
	m_hEvent = hEvent;
}

CharacterSubtractLife::CharacterSubtractLife() {
	m_hEvent = NULL;
}

void CharacterSubtractLife::DisableHook() {
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourDetach((PVOID*)&originalMethod, HookedMethod);
	DetourTransactionCommit();
}

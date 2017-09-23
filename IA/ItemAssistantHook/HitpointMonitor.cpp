#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include <detours.h>
#include "HitpointMonitor.h"
#include "Globals.h"

HANDLE HitpointMonitor::m_hEvent;
DataQueue* HitpointMonitor::m_dataQueue;
HitpointMonitor::OriginalMethodPtr HitpointMonitor::originalMethod;
HitpointMonitor::GetObjectIdMethodPtr HitpointMonitor::GetObjectId;
int HitpointMonitor::m_offset = 0;

/*
GAME::Character::SubtractLife(*(GAME::Character **)(this_ + 4), *(float *)&damageAmount, a5, 0.0, 1);

*((float *)v5 + 564) = *((float *)v5 + 564) - damageTaken;
03F78C
03F788
03F783

.text:10040375                 lea     ecx, [edi+8B4h] ; this
.text:1004038C                 movss   dword ptr [ecx+1Ch], xmm0

lea
8D 8F B4 08 00 00				8D 8F CC  08 00 00
LL LL ++ ++ ++ ++

F3 0F 11 41 1C				exact match on both
movss -- -- ++


.text:10040BE0				 lea     ecx, [edi+8CCh] ; thi
.text:10040BE0				 movss   dword ptr [ecx+1Ch], xmm0

// Expansion dll
C6 81 6D 0D 00 00 01 C3  CC CC CC CC CC CC CC CC
55 8B EC 8A 45 08 88 81  0C 16 00 00 5D C2 04 00
8A 81 0C 16 00 00 C3 CC  CC CC CC CC CC CC CC CC
55 8B EC 83 EC 10 80 7D  10 00 57 8B F9 75 31 FF
15 94 60 3A 10 8B 0D E4  D6 62 10 50 E8 AF 67 19
00 84 C0 75 0E 8A 87 B4  12 00 00 84 C0 0F 85 D4
00 00 00 80 BF B5 12 00  00 00 0F 85 C7 00 00 00
F3 0F 10 4D 08 8D 8F CC  08 00 00 0F 57 D2 0F 2F
CA 72 1B F3 0F 10 41 1C  F3 0F 5C C1 F3 0F 11 41
1C 0F 28 C1 F3 0F 58 41  24 F3 0F 11 41 24 80 7D
14 00 53 8B 5D 0C 56 C6  81 A0 04 00 00 01 74 36
83 3B 01 75 31 F3 0F 10  87 E8 08 00 00 F3 0F 5F
C2 0F 2F D0 72 20 E8 75  D6 01 00 D8 8F 04 16 00
00 F3 0F 10 45 08 D9 5D  10 0F 2F 45 10 72 07 C6
87 0C 16 00 00 01 53 8D  45 F8 50 8D 8F 44 0F 00
00 E8 BA 83 24 00 53 8D  45 F0 50 8D 8F 44 0F 00
00 E8 AA 83 24 00 8B 45  F8 F3 0F 10 4D 08 5E F3
0F 10 40 18 8B 45 F0 F3  0F 58 C1 F3 0F 11 40 18
83 3B 01 5B 75 11 0F 2F  8F 80 0E 00 00 76 08 F3
0F 11 8F 80 0E 00 00 5F  8B E5 5D C2 10 00 CC CC
55 8B EC 80 B9 B6 12 00  00 00 75 30 F3 0F 10 81
08 09 00 00 F3 0F 10 4D  08 F3 0F 58 C1 C6 81 6C
0D 00 00 01 F3 0F 11 81  08 09 00 00 F3 0F 58 89
64 0F 00 00 F3 0F 11 89  64 0F 00 00 5D C2 04 00
55 8B EC 56 57 8B F9 8B  B7 94 0D 00 00 FF 15 64
60 3A 10 56 8B C8 E8 95  9F FC FF 8B F0 85 F6 74
23 8B 16 8B CE 68 78 6F  62 10 FF 12 8B C8 E8 ED
*/
bool HitpointMonitor::DetectOffset() {
	SIZE_T bytesRead = 0;
	HANDLE hProcess = GetCurrentProcess();
	const size_t resultSize = 351;
	unsigned char result[resultSize] = { 0 };

	char* addr = (char*)GetProcAddress(::GetModuleHandle("Game.dll"), "?SubtractLife@Character@GAME@@QAEXMABUPlayStatsDamageType@2@_N_N@Z");

	int lea = 0;
	if (ReadProcessMemory(hProcess, addr, (void*)&result, 350, &bytesRead) != 0) {
		for (int i = 0; i < bytesRead - 5; i++) {
			if ((int)result[i] == 0x8D && (int)result[i + 1] == 0x8F) {
				memcpy(&lea, &result[i + 2], 4);
			} 
			else if (lea != 0 && result[i] == 0xF3 && result[i + 1] == 0x0F && result[i + 3] == 0x41) {
				lea += result[i + 4];


				DataItemPtr item(new DataItem(TYPE_PlayerHealthOffsetDetected, sizeof(lea), (char*)&lea));
				m_dataQueue->push(item);
				SetEvent(m_hEvent);

				m_offset = lea;
				return true;
			}
		}
	}

	DataItemPtr item(new DataItem(TYPE_ErrorDetectingPlayerHealthOffset, resultSize, (char*)result));
	m_dataQueue->push(item);
	SetEvent(m_hEvent);
	return false;
}

void HitpointMonitor::EnableHook() {
	if (DetectOffset()) {
		GetObjectId = (GetObjectIdMethodPtr)GetProcAddress(::GetModuleHandle("Engine.dll"), "?GetObjectId@Object@GAME@@QBEIXZ");
		originalMethod = (OriginalMethodPtr)GetProcAddress(::GetModuleHandle("Game.dll"), "?UpdateSelf@Player@GAME@@UAEXH@Z");
		DetourTransactionBegin();
		DetourUpdateThread(GetCurrentThread());
		DetourAttach((PVOID*)&originalMethod, HookedMethod);
		DetourTransactionCommit();
	}
}

HitpointMonitor::HitpointMonitor(DataQueue* dataQueue, HANDLE hEvent) {
	HitpointMonitor::m_dataQueue = dataQueue;
	HitpointMonitor::m_hEvent = hEvent;
}

HitpointMonitor::HitpointMonitor() {
	HitpointMonitor::m_hEvent = NULL;
}

void HitpointMonitor::DisableHook() {
	if (m_offset != 0) {
		LONG res1 = DetourTransactionBegin();
		LONG res2 = DetourUpdateThread(GetCurrentThread());
		DetourDetach((PVOID*)&originalMethod, HookedMethod);
		DetourTransactionCommit();
	}
}

int lastSend = 0;
void* __fastcall HitpointMonitor::HookedMethod(void* This, void* notUsed, int val) {
	void* v = originalMethod(This, val); 

	if (lastSend++ > 30) {
		SIZE_T bytesRead = 0;
		HANDLE hProcess = GetCurrentProcess();
		const size_t resultSize = 8;
		unsigned char result[resultSize] = { 0 };

		if (ReadProcessMemory(hProcess, (char*)This + m_offset, (char*)&result + 4, 4, &bytesRead) != 0) {
			int id = GetObjectId((void*)This);
			memcpy(result, &id, sizeof(id));

			DataItemPtr item(new DataItem(TYPE_HitpointMonitor, resultSize, (char*)&result));
			m_dataQueue->push(item);
			SetEvent(m_hEvent);

		}
		lastSend = 0;
	}
	return v;
}
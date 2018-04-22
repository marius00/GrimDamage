#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include <detours.h>
#include "ApplyDamageHook.h"

HANDLE ApplyDamageHook::m_hEvent;
DataQueue* ApplyDamageHook::m_dataQueue;
ApplyDamageHook::OriginalMethodPtr ApplyDamageHook::originalMethod;
int CharacterApplyDamage = 10101012;

void ApplyDamageHook::EnableHook() {

	// double __userpurge GAME::CombatManager::ApplyDamage@<st0>(int a1@<eax>, int _this@<ecx>, double result@<st0>, double st6_0@<st1>, double st5_0@<st2>, double st4_0@<st3>, double st3_0@<st4>, double a8@<st5>, int a4, struct GAME::PlayStatsDamageType *a5, int a6, int a7)
	// bool GAME::CombatManager::ApplyDamage(float,struct GAME::PlayStatsDamageType const &,enum GAME::CombatAttributeType,class mem::vector<unsigned int> const &)
	originalMethod = (OriginalMethodPtr)GetProcAddress(::GetModuleHandle("Game.dll"), "?ApplyDamage@CombatManager@GAME@@QAE_NMABUPlayStatsDamageType@2@W4CombatAttributeType@2@ABV?$vector@I@mem@@@Z");
	if (originalMethod == NULL) {
		DataItemPtr item(new DataItem(CharacterApplyDamage, 0, NULL));
		m_dataQueue->push(item);
		SetEvent(m_hEvent);
	}

	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach((PVOID*)&originalMethod, HookedMethod);
	DetourTransactionCommit();
}

ApplyDamageHook::ApplyDamageHook(DataQueue* dataQueue, HANDLE hEvent) {
	m_dataQueue = dataQueue;
	m_hEvent = hEvent;
}

ApplyDamageHook::ApplyDamageHook() {
	m_hEvent = NULL;
}

void ApplyDamageHook::DisableHook() {
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourDetach((PVOID*)&originalMethod, HookedMethod);
	DetourTransactionCommit();
}


/* double __userpurge GAME::CombatManager::ApplyDamage@<st0>(
* int a1@<eax>,
* int _this@<ecx>,
* double result@<st0>,
*  double st6_0@<st1>,
*  double st5_0@<st2>,
*  double st4_0@<st3>,
*  double st3_0@<st4>,
*  double a8@<st5>,
*  int a4,
*  struct GAME::PlayStatsDamageType *a5,
*  int a6,
*  int a7
*  )
* */
// bool GAME::CombatManager::ApplyDamage(float,struct GAME::PlayStatsDamageType const &,enum GAME::CombatAttributeType,class mem::vector<unsigned int> const &)
void* __fastcall ApplyDamageHook::HookedMethod(void* This, void* _, float f, int* PlayStatsDamageType, int CombatAttributeType, void* Vector) {
	const int buffsize = sizeof(float) + sizeof(int) + sizeof(int);
	char buffer[buffsize] = { 0 };

	// float, struct, enum, vector<int>

	int pos = 0;

	SIZE_T bytesRead = 0;
	HANDLE hProcess = GetCurrentProcess();

	ReadProcessMemory(hProcess, (void*)&f, (char*)&buffer + pos, 4, &bytesRead);
	pos += 4;

	ReadProcessMemory(hProcess, (void*)PlayStatsDamageType, (char*)&buffer + pos, 4, &bytesRead);
	pos += 4;

	ReadProcessMemory(hProcess, (void*)&CombatAttributeType, (char*)&buffer + pos, 4, &bytesRead);

	// OBS: Vector ignored

	DataItemPtr item(new DataItem(CharacterApplyDamage, buffsize, (char*)&buffer));
	m_dataQueue->push(item);
	SetEvent(m_hEvent);

	void* v = originalMethod(This, f, PlayStatsDamageType, CombatAttributeType, Vector);
	return v;
}
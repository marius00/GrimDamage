#include <stdio.h>
#include <stdlib.h>
#include "../MessageType.h"
#include <detours.h>
#include "LoggerHook.h"

HANDLE LoggerHook::m_hEvent;
DataQueue* LoggerHook::m_dataQueue;
LoggerHook::OriginalMethodPtr LoggerHook::originalMethod;


void LoggerHook::EnableHook() {

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
	DetourAttach((PVOID*)&originalMethod, func_hook);
	DetourTransactionCommit();
}

LoggerHook::LoggerHook(DataQueue* dataQueue, HANDLE hEvent) {
	m_dataQueue = dataQueue;
	m_hEvent = hEvent;
}

LoggerHook::LoggerHook() {
	m_hEvent = NULL;
}

void LoggerHook::DisableHook() {
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourDetach((PVOID*)&originalMethod, func_hook);
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
void* __fastcall LoggerHook::HookedMethod(void* This, void* _, float f, int PlayStatsDamageType, int CombatAttributeType, void* Vector) {
	const int buffsize = sizeof(void*) + sizeof(float) + sizeof(int) + sizeof(int);
	char buffer[buffsize] = { 0 };

	int ptr = 0;


	memcpy(&buffer + ptr, (char*)&f, sizeof(float));
	ptr += 4;

	memcpy(&buffer + ptr, (char*)&PlayStatsDamageType, sizeof(int));
	ptr += 4;

	int attr = 0;
	__asm {
		push eax
		mov eax, [esp + 94h]
		mov attr, eax
		pop eax
	}

	memcpy(&buffer + ptr, (char*)&attr, sizeof(int));
	ptr += 4;

	int _this = -1;
	__asm {
		mov _this, ecx
	}
	memcpy(&buffer + ptr, (char*)&_this, sizeof(int));
	ptr += sizeof(_this);


	DataItemPtr item(new DataItem(CharacterApplyDamage, buffsize, (char*)&buffer));
	m_dataQueue->push(item);
	SetEvent(m_hEvent);

	void* v = originalMethod(This, f, PlayStatsDamageType, CombatAttributeType, Vector);
	return v;
}
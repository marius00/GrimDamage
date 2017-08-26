//char __userpurge GAME::CombatManager::TakeAttack@<al>(GAME::CombatManager *this@<ecx>, double st1_0@<st6>, double st2_0@<st5>, double st3_0@<st4>, double a5@<st3>, double a6@<st2>, double a7@<st1>, double a8@<st0>, struct GAME::ParametersCombat *a2, struct GAME::SkillManager *a3, struct GAME::CharacterBio *a4)
/*
#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include "MessageType.h"
#include <detours.h>
#include "TakeAttack.h"
#include <windef.h>
#include "../SaveTransferStash.h"
#include "../MessageType.h"

HANDLE TakeAttack::m_hEvent;
DataQueue* TakeAttack::m_dataQueue;
TakeAttack::OriginalMethodPtr TakeAttack::originalMethod;


void TakeAttack::EnableHook() {

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

TakeAttack::TakeAttack(DataQueue* dataQueue, HANDLE hEvent) {
	m_dataQueue = dataQueue;
	m_hEvent = hEvent;
}

TakeAttack::TakeAttack() {
	m_hEvent = NULL;
}

void TakeAttack::DisableHook() {
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
* /
// bool GAME::CombatManager::ApplyDamage(float,struct GAME::PlayStatsDamageType const &,enum GAME::CombatAttributeType,class mem::vector<unsigned int> const &)
void* __fastcall TakeAttack::HookedMethod(void* This, void* _, float f, int PlayStatsDamageType, int CombatAttributeType, void* Vector) {
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
}*/
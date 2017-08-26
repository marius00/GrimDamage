#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include "MessageType.h"
#include <detours.h>
#include "SaveTransferStash.h"

HANDLE SaveTransferStash::m_hEvent;
DataQueue* SaveTransferStash::m_dataQueue;
SaveTransferStash::OriginalMethodPtr SaveTransferStash::originalMethod;


void __stdcall func_hook_payload( /*void* This, float f, int PlayStatsDamageType, int CombatAttributeType */) {
	
	const int buffsize = sizeof(void*) + sizeof(float) + sizeof(int) + sizeof(int) * 2;
	char buffer[buffsize] = { 0 };

	int ptr = 0;


	//memcpy(&buffer + ptr, (char*)&f, sizeof(float));
	ptr += 4;
	/*
	memcpy(&buffer + ptr, (char*)&PlayStatsDamageType, sizeof(int));
	ptr += 4;

	memcpy(&buffer + ptr, (char*)&CombatAttributeType, sizeof(int));
	ptr += 4;
	
	int _this = (int)This;
	memcpy(&buffer + ptr, (char*)&_this, sizeof(int));
	ptr += sizeof(_this);*/
	/*
	DataItemPtr item(new DataItem(CharacterApplyDamage, buffsize, (char*)&buffer));
	SaveTransferStash::m_dataQueue->push(item);
	SetEvent(SaveTransferStash::m_hEvent);
	*/
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
// https://stackoverflow.com/questions/4099026/how-to-hook-usercall-userpurge-spoils-functions
__declspec(naked) void func_hook()
{
	__asm {
		
		//push ebp  // why<?
		//mov ebp, esp // why?
		/*
		push dword ptr[ebp + 0x0C] // a6
		push dword ptr[ebp + 0x08] // a5
		push dword ptr[ebp + 0x04] // a4
		// st0,st1,st2,st3,st4,st5
		push ecx // this
		*/
		call func_hook_payload
		//leave
		//ret // note: __usercall is cdecl-like

		jmp SaveTransferStash::originalMethod
	}

}


void SaveTransferStash::EnableHook() {

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

SaveTransferStash::SaveTransferStash(DataQueue* dataQueue, HANDLE hEvent) {
	m_dataQueue = dataQueue;
	m_hEvent = hEvent;
}

SaveTransferStash::SaveTransferStash() {
	m_hEvent = NULL;
}

void SaveTransferStash::DisableHook() {
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
void* __fastcall SaveTransferStash::HookedMethod(void* This, void* _, float f, int PlayStatsDamageType, int CombatAttributeType, void* Vector) {
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
		mov eax, [esp+94h]
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
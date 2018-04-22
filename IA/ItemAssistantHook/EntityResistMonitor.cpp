#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include <detours.h>
#include "EntityResistMonitor.h"
#include "Globals.h"

#define Fire 6
#define Cold 5
#define Lightning 8
#define Poison 7
#define Pierce 4
#define Bleed 15
#define Vitality 9
#define Aether 11
#define Chaos 10

HANDLE EntityResistMonitor::m_hEvent;
DataQueue* EntityResistMonitor::m_dataQueue;
EntityResistMonitor::CharacterTakeAttackPtr EntityResistMonitor::originalCharacterTakeAttackMethod;
EntityResistMonitor::GetTotalDefenseTypePtr EntityResistMonitor::originalGetTotalDefenseTypeMethod;
EntityResistMonitor::GetObjectIdMethodPtr EntityResistMonitor::GetObjectId;
EntityResistMonitor::CharAttributeMod_TotalSpeed_AddToAccumulatorPtr EntityResistMonitor::originalCharAttributeMod_TotalSpeed_AddToAccumulatorMethod;
EntityResistMonitor::CharAttributeAccumulator_ExecuteDefensePtr EntityResistMonitor::originalCharAttributeAccumulator_ExecuteDefenseMethod;
EntityResistMonitor::SkillBuff_DebufTrap_GetResistancePtr EntityResistMonitor::originalSkillBuff_DebufTrap_GetResistanceMethod;
EntityResistMonitor::SkillBuff_DebufFreeze_GetResistancePtr EntityResistMonitor::originalSkillBuff_DebufFreeze_GetResistanceMethod;
EntityResistMonitor::Character_GetAllDefenseAttributesPtr EntityResistMonitor::originalCharacter_GetAllDefenseAttributesMethod;
EntityResistMonitor::SkillManager_GetDefenseAttributesPtr EntityResistMonitor::originalSkillManager_GetDefenseAttributesMethod;
EntityResistMonitor::CombatManagerTakeAttackPtr EntityResistMonitor::originalCombatManagerTakeAttackPtr;

EntityResistMonitor::CharacterBio_GetAllDefenseAttributesPtr EntityResistMonitor::originalCharacterBio_GetAllDefenseAttributesMethod;
EntityResistMonitor::ItemEquipment_GetAllDefenseAttributesPtr EntityResistMonitor::originalItemEquipment_GetAllDefenseAttributesMethod;
EntityResistMonitor::CombatAttributeAccumulator_ProcessDefense EntityResistMonitor::originalCombatAttributeAccumulator_ProcessDefenseMethod;
EntityResistMonitor::CombatAttributeAccumulator_Deconstructor EntityResistMonitor::originalCombatAttributeAccumulator_Deconstructor;
int EntityResistMonitor::previousId;

void EntityResistMonitor::EnableHook() {
	GetObjectId = (GetObjectIdMethodPtr)GetProcAddress(::GetModuleHandle("Engine.dll"), "?GetObjectId@Object@GAME@@QBEIXZ");
	originalCharacterTakeAttackMethod = (CharacterTakeAttackPtr)GetProcAddress(::GetModuleHandle("Game.dll"), "?TakeAttack@Character@GAME@@UAE_NAAVParametersCombat@2@@Z");
	originalCombatManagerTakeAttackPtr = (CombatManagerTakeAttackPtr)GetProcAddress(::GetModuleHandle("Game.dll"), "?TakeAttack@CombatManager@GAME@@QAE_NAAVParametersCombat@2@AAVSkillManager@2@AAVCharacterBio@2@@Z");

	originalGetTotalDefenseTypeMethod = (GetTotalDefenseTypePtr)GetProcAddress(::GetModuleHandle("Game.dll"), "?GetTotalDefenseType@CombatAttributeAccumulator@GAME@@QAEMW4CombatAttributeType@2@@Z");
	

	originalCharAttributeMod_TotalSpeed_AddToAccumulatorMethod = (CharAttributeMod_TotalSpeed_AddToAccumulatorPtr)GetProcAddress(::GetModuleHandle("Game.dll"), "?AddToAccumulator@CharAttributeMod_TotalSpeed@GAME@@UBEXAAVCharAttributeAccumulator@2@I@Z");
	originalCharAttributeAccumulator_ExecuteDefenseMethod = (CharAttributeAccumulator_ExecuteDefensePtr)GetProcAddress(::GetModuleHandle("Game.dll"), "?ExecuteDefense@CharAttributeAccumulator@GAME@@QAEMW4CharAttributeType@2@M@Z");
	originalSkillBuff_DebufTrap_GetResistanceMethod = (SkillBuff_DebufTrap_GetResistancePtr)GetProcAddress(::GetModuleHandle("Game.dll"), "?GetResistance@SkillBuff_DebufFreeze@GAME@@MAEMAAVCombatAttributeAccumulator@2@@Z");
	originalSkillBuff_DebufFreeze_GetResistanceMethod = (SkillBuff_DebufFreeze_GetResistancePtr)GetProcAddress(::GetModuleHandle("Game.dll"), "?GetResistance@SkillBuff_DebufTrap@GAME@@MAEMAAVCombatAttributeAccumulator@2@@Z");
	
	originalCharacter_GetAllDefenseAttributesMethod = (Character_GetAllDefenseAttributesPtr)GetProcAddress(::GetModuleHandle("Game.dll"), "?GetAllDefenseAttributes@Character@GAME@@QBEXAAVCombatAttributeAccumulator@2@@Z");
	originalSkillManager_GetDefenseAttributesMethod = (Character_GetAllDefenseAttributesPtr)GetProcAddress(::GetModuleHandle("Game.dll"), "?GetDefenseAttributes@SkillManager@GAME@@QBEXAAVCombatAttributeAccumulator@2@@Z");

	originalCharacterBio_GetAllDefenseAttributesMethod = (CharacterBio_GetAllDefenseAttributesPtr)GetProcAddress(::GetModuleHandle("Game.dll"), "?GetDefenseAttributes@CharacterBio@GAME@@QBEXAAVCombatAttributeAccumulator@2@@Z");
	originalItemEquipment_GetAllDefenseAttributesMethod = (ItemEquipment_GetAllDefenseAttributesPtr)GetProcAddress(::GetModuleHandle("Game.dll"), "?GetDefenseAttributes@ItemEquipment@GAME@@UBEXAAVCombatAttributeAccumulator@2@@Z");
	originalCombatAttributeAccumulator_ProcessDefenseMethod = (CombatAttributeAccumulator_ProcessDefense)GetProcAddress(::GetModuleHandle("Game.dll"), "?ProcessDefense@CombatAttributeAccumulator@GAME@@QAEXABVCharacter@2@ABUReductionInfo@2@MM@Z");

	originalCombatAttributeAccumulator_Deconstructor = (CombatAttributeAccumulator_Deconstructor)GetProcAddress(::GetModuleHandle("Game.dll"), "??1CombatAttributeAccumulator@GAME@@UAE@XZ");


	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach((PVOID*)&originalCharacterTakeAttackMethod, HookedCharacterTakeAttack);
	DetourAttach((PVOID*)&originalCombatManagerTakeAttackPtr, HookedCombatManagerTakeAttackPtr);
	DetourAttach((PVOID*)&originalCombatAttributeAccumulator_Deconstructor, HookedCombatAttributeAccumulator_Deconstructor);


	DetourAttach((PVOID*)&originalCharacterBio_GetAllDefenseAttributesMethod, HookedCharacterBio_GetAllDefenseAttributes);
	DetourAttach((PVOID*)&originalItemEquipment_GetAllDefenseAttributesMethod, HookedItemEquipment_GetAllDefenseAttributes);
	DetourAttach((PVOID*)&originalCombatAttributeAccumulator_ProcessDefenseMethod, HookedCombatAttributeAccumulator_ProcessDefense);
	
	DetourAttach((PVOID*)&originalGetTotalDefenseTypeMethod, HookedGetTotalDefenseType); // this will be it
	
	DetourAttach((PVOID*)&originalCharAttributeMod_TotalSpeed_AddToAccumulatorMethod, HookedCharAttributeMod_TotalSpeed_AddToAccumulator);
	DetourAttach((PVOID*)&originalCharAttributeAccumulator_ExecuteDefenseMethod, HookedCharAttributeAccumulator_ExecuteDefense);
	DetourAttach((PVOID*)&originalSkillBuff_DebufTrap_GetResistanceMethod, HookedSkillBuff_DebufTrap_GetResistance);
	DetourAttach((PVOID*)&originalSkillBuff_DebufFreeze_GetResistanceMethod, HookedSkillBuff_DebufFreeze_GetResistance);

	DetourAttach((PVOID*)&originalCharacter_GetAllDefenseAttributesMethod, HookedCharacter_GetAllDefenseAttributes);
	DetourAttach((PVOID*)&originalSkillManager_GetDefenseAttributesMethod, HookedSkillManager_GetDefenseAttributes);
	
	DetourTransactionCommit();

	//DetourTransactionBegin();
	//DetourUpdateThread(GetCurrentThread());
	//DetourAttach((PVOID*)&originalCharAttributeMod_TotalSpeed_AddToAccumulatorMethod, HookedCharAttributeMod_TotalSpeed_AddToAccumulator);
	//DetourAttach((PVOID*)&originalCharAttributeAccumulator_ExecuteDefenseMethod, HookedCharAttributeAccumulator_ExecuteDefense);
	//DetourAttach((PVOID*)&originalSkillBuff_DebufTrap_GetResistanceMethod, HookedSkillBuff_DebufTrap_GetResistance);
	//DetourAttach((PVOID*)&originalSkillBuff_DebufFreeze_GetResistanceMethod, HookedSkillBuff_DebufFreeze_GetResistance);
	//DetourTransactionCommit();
}

EntityResistMonitor::EntityResistMonitor(DataQueue* dataQueue, HANDLE hEvent) {
	EntityResistMonitor::m_dataQueue = dataQueue;
	EntityResistMonitor::m_hEvent = hEvent;
	EntityResistMonitor::previousId = 0;
}

EntityResistMonitor::EntityResistMonitor() {
	EntityResistMonitor::m_hEvent = NULL;
}

void EntityResistMonitor::DisableHook() {
	LONG res1 = DetourTransactionBegin();
	LONG res2 = DetourUpdateThread(GetCurrentThread());
	DetourDetach((PVOID*)&originalCharacterTakeAttackMethod, HookedCharacterTakeAttack);
	DetourDetach((PVOID*)&originalCombatManagerTakeAttackPtr, HookedCombatManagerTakeAttackPtr);
	DetourDetach((PVOID*)&originalGetTotalDefenseTypeMethod, HookedGetTotalDefenseType);
	DetourDetach((PVOID*)&originalCharAttributeAccumulator_ExecuteDefenseMethod, HookedCharAttributeAccumulator_ExecuteDefense);
	DetourTransactionCommit();
}
void* __fastcall EntityResistMonitor::HookedCombatManagerTakeAttackPtr(void* This, void* _, void* classParametersCombat, void* skillManager, void* characterBio) {
	void* v = originalCombatManagerTakeAttackPtr(This, classParametersCombat, skillManager, characterBio);

	DataItemPtr item(new DataItem(999111, 0, 0));
	m_dataQueue->push(item);
	SetEvent(m_hEvent);

	return v;
}

void* EntityResistMonitor::HookedCharacterBio_GetAllDefenseAttributes(void* This, void* _, void* uk) {
	DataItemPtr item(new DataItem(929002, 0, 0));
	m_dataQueue->push(item);
	SetEvent(m_hEvent);

	void* v = originalCharacterBio_GetAllDefenseAttributesMethod(This, uk);
	return v;
}

void* EntityResistMonitor::HookedItemEquipment_GetAllDefenseAttributes(void* This, void* _, void* uk) {
	DataItemPtr item(new DataItem(929003, 0, 0));
	m_dataQueue->push(item);
	SetEvent(m_hEvent);

	void* v = originalItemEquipment_GetAllDefenseAttributesMethod(This, uk);
	return v;
}

void* EntityResistMonitor::HookedCombatAttributeAccumulator_ProcessDefense(void* This, void* _, void* character, void* reductionInfo, float a4, float a5) {
	
	const size_t bufsize = sizeof(int) + sizeof(float) * 9;
	char buffer[bufsize] = { 0 };
	memcpy(&buffer, (char*)&previousId, 4);

	//int resists[] = { Fire, Cold, Lightning, Poison, Pierce, Bleed, Vitality, Aether, Chaos };
	int resists[] = { Fire };
	int numResists = sizeof(resists) / sizeof(int);

	for (int idx = 0; idx < numResists; idx++) {
		float resist = originalGetTotalDefenseTypeMethod(This, (void*)resists[idx]);
		memcpy(&buffer + 4 * (idx + 1), (char*)&resist, 4);

	}
	
	
	DataItemPtr item(new DataItem(929001, bufsize, (char*)buffer));
	m_dataQueue->push(item);
	SetEvent(m_hEvent);



	// This may be the way fowards.
	void* v = originalCombatAttributeAccumulator_ProcessDefenseMethod(This, character, reductionInfo, a4, a5);
	return v;
}


void* safetyMeasure = 0;
void* EntityResistMonitor::HookedCombatAttributeAccumulator_Deconstructor(void* This, void* _) {

	if (This == safetyMeasure) {
		int s = (int)This;
		DataItemPtr item(new DataItem(929222, 4, (char*)&s));
		m_dataQueue->push(item);
		SetEvent(m_hEvent);
		safetyMeasure = 0;
	}

	void* v = originalCombatAttributeAccumulator_Deconstructor(This);
	return v;
}


void* __fastcall EntityResistMonitor::HookedCharacterTakeAttack(void* This, void* _, void* ClassParametersCombat) {
	DataItemPtr item(new DataItem(999001, 0, 0)); 
	m_dataQueue->push(item);
	SetEvent(m_hEvent);
	void* v = originalCharacterTakeAttackMethod(This, ClassParametersCombat);
	previousId = GetObjectId((void*)This);
	return v;
}

/************************************************************************
/* When the "C" character menu is open this spams
 * Uncapped
 * Capped
 * Capped
 * => Next resist
 *                                                                      *
/************************************************************************/
float __fastcall EntityResistMonitor::HookedGetTotalDefenseType(void* This, void* _, void* classCombatAttributeType) {
	float v = originalGetTotalDefenseTypeMethod(This, classCombatAttributeType);
	
	const size_t resultSize = sizeof(int) + sizeof(float) * 2;
	char result[resultSize] = { 0 };

	memcpy(result + 0, &previousId, sizeof(previousId));
	memcpy(result + 4, &classCombatAttributeType, sizeof(classCombatAttributeType));
	memcpy(result + 8, &v, sizeof(float));

	DataItemPtr item(new DataItem(10101010, resultSize, (char*)&result));
	m_dataQueue->push(item);
	SetEvent(m_hEvent);

	safetyMeasure = This;
	
	return v;
}

void* __fastcall EntityResistMonitor::HookedCharAttributeMod_TotalSpeed_AddToAccumulator(void* This, void* _, void* a2, void* a3) {
	void* result = originalCharAttributeMod_TotalSpeed_AddToAccumulatorMethod(This, a2, a3);
	previousId = 0;
	return result;
}
void* __fastcall EntityResistMonitor::HookedCharAttributeAccumulator_ExecuteDefense(void* This, void* _, void* a2, void* a3) {
	void* result = originalCharAttributeAccumulator_ExecuteDefenseMethod(This, a2, a3);	
	previousId = 0;	

	DataItemPtr item(new DataItem(929022, 0, 0));
	m_dataQueue->push(item);
	SetEvent(m_hEvent);

	return result;
}
void* __fastcall EntityResistMonitor::HookedSkillBuff_DebufTrap_GetResistance(void* This, void* _) {
	void* result = originalSkillBuff_DebufTrap_GetResistanceMethod(This);
	previousId = 0;
	return result;
}
void* __fastcall EntityResistMonitor::HookedSkillBuff_DebufFreeze_GetResistance(void* This, void* _) {
	void* result = originalSkillBuff_DebufFreeze_GetResistanceMethod(This);
	previousId = 0;
	return result;
}

void* __fastcall EntityResistMonitor::HookedCharacter_GetAllDefenseAttributes(void* This, void* _, void* arg1) {
	previousId = GetObjectId((void*)This);

	void* result = originalCharacter_GetAllDefenseAttributesMethod(This, arg1);
	const size_t bufsize = sizeof(int) + sizeof(float) * 9;
	char buffer[bufsize] = { 0 };
	memcpy(&buffer, (char*)&previousId, 4);

	//int resists[] = { Fire, Cold, Lightning, Poison, Pierce, Bleed, Vitality, Aether, Chaos };
	int resists[] = { Fire };
	int numResists = sizeof(resists) / sizeof(int);
	
	for (int idx = 0; idx < numResists; idx++) {
		float resist = originalGetTotalDefenseTypeMethod(arg1, (void*)resists[idx]);
		memcpy(&buffer + 4 * (idx+1), (char*)&resist, 4);

	}
	
	DataItemPtr item(new DataItem(999002, bufsize, (char*)&buffer));
	m_dataQueue->push(item);
	SetEvent(m_hEvent);
	// Call GAME::CombatAttributeAccumulator::GetTotalDefenseType((int)&v216, COERCE_FLOAT(62));
	// For each desired skill
	// v16 == 'arg1'

	return result;
}
void* __fastcall EntityResistMonitor::HookedSkillManager_GetDefenseAttributes(void* This, void* _, void* a1) {
	/*
	DataItemPtr item(new DataItem(999003, sizeof(previousId), (char*)&previousId));
	m_dataQueue->push(item);
	SetEvent(m_hEvent);
	return originalSkillManager_GetDefenseAttributesMethod(This, a1);
	*/
	// Call GAME::CombatAttributeAccumulator::GetTotalDefenseType((int)&v216, COERCE_FLOAT(62));
	// For each desired skill
	// v16 == 'a1'







	void* result = originalSkillManager_GetDefenseAttributesMethod(This, a1);
	const size_t bufsize = sizeof(int) + sizeof(float) * 9;
	char buffer[bufsize] = { 0 };
	memcpy(&buffer, (char*)&previousId, 4);

	//int resists[] = { Fire, Cold, Lightning, Poison, Pierce, Bleed, Vitality, Aether, Chaos };
	int resists[] = { Fire };
	int numResists = sizeof(resists) / sizeof(int);
	for (int idx = 0; idx < numResists; idx++) {
		float resist = HookedGetTotalDefenseType(a1, NULL, (void*)resists[idx]);
		memcpy(&buffer + 4 * (idx + 1), (char*)&resist, 4);

	}

	DataItemPtr item(new DataItem(999003, bufsize, (char*)&buffer));
	m_dataQueue->push(item);
	SetEvent(m_hEvent);
	// Call GAME::CombatAttributeAccumulator::GetTotalDefenseType((int)&v216, COERCE_FLOAT(62));
	// For each desired skill
	// v16 == 'arg1'

	return result;
}
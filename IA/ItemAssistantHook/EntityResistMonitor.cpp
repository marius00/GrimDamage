#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include <detours.h>
#include "EntityResistMonitor.h"
#include "Globals.h"

HANDLE EntityResistMonitor::m_hEvent;
DataQueue* EntityResistMonitor::m_dataQueue;
EntityResistMonitor::CharacterTakeAttackPtr EntityResistMonitor::originalCharacterTakeAttackMethod;
EntityResistMonitor::GetTotalDefenseTypePtr EntityResistMonitor::originalGetTotalDefenseTypeMethod;
EntityResistMonitor::GetObjectIdMethodPtr EntityResistMonitor::GetObjectId;
int EntityResistMonitor::previousId;

void EntityResistMonitor::EnableHook() {
	GetObjectId = (GetObjectIdMethodPtr)GetProcAddress(::GetModuleHandle("Engine.dll"), "?GetObjectId@Object@GAME@@QBEIXZ");
	originalCharacterTakeAttackMethod = (CharacterTakeAttackPtr)GetProcAddress(::GetModuleHandle("Game.dll"), "?TakeAttack@Character@GAME@@UAE_NAAVParametersCombat@2@@Z");
	originalGetTotalDefenseTypeMethod = (GetTotalDefenseTypePtr)GetProcAddress(::GetModuleHandle("Game.dll"), "?GetTotalDefenseType@CombatAttributeAccumulator@GAME@@QAEMW4CombatAttributeType@2@@Z");
	
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach((PVOID*)&originalCharacterTakeAttackMethod, HookedCharacterTakeAttack);
	DetourAttach((PVOID*)&originalGetTotalDefenseTypeMethod, HookedGetTotalDefenseType);
	DetourTransactionCommit();
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
	DetourDetach((PVOID*)&originalGetTotalDefenseTypeMethod, HookedGetTotalDefenseType);
	DetourTransactionCommit();
}

void* __fastcall EntityResistMonitor::HookedCharacterTakeAttack(void* This, void* _, void* ClassParametersCombat) {
	void* v = originalCharacterTakeAttackMethod(This, ClassParametersCombat);

	const size_t resultSize = sizeof(int);
	char result[resultSize] = { 0 };

	previousId = GetObjectId((void*)This);
	return v;
}

float __fastcall EntityResistMonitor::HookedGetTotalDefenseType(void* This, void* _, float classCombatAttributeType) {
	float v = originalGetTotalDefenseTypeMethod(This, classCombatAttributeType);

	const size_t resultSize = sizeof(int) + sizeof(float) * 2;
	char result[resultSize] = { 0 };

	memcpy(result + 0, &previousId, sizeof(previousId));
	memcpy(result + 4, &classCombatAttributeType, sizeof(classCombatAttributeType));
	memcpy(result + 8, &v, sizeof(float));

	DataItemPtr item(new DataItem(10101010, resultSize, (char*)&result));
	m_dataQueue->push(item);
	SetEvent(m_hEvent);

	return v;
}
#pragma once
#include <windows.h>
#include "DataQueue.h"
#include "BaseMethodHook.h"

/************************************************************************
/************************************************************************/
class EntityResistMonitor : public BaseMethodHook {
public:
	EntityResistMonitor();
	EntityResistMonitor(DataQueue* dataQueue, HANDLE hEvent);
	void EnableHook();
	void DisableHook();

private:
	typedef int(__thiscall *GetObjectIdMethodPtr)(void*);
	typedef void* (__thiscall *CharacterTakeAttackPtr)(void*, void* classParametersCombat);
	typedef float (__thiscall *GetTotalDefenseTypePtr)(void*, float classCombatAttributeType);

	// Unsetters / unhandled methods
	typedef void* (__thiscall *CharAttributeMod_TotalSpeed_AddToAccumulatorPtr)(void*, void* CharAttributeAccumulator, void* a3);
	typedef void* (__thiscall *CharAttributeAccumulator_ExecuteDefensePtr)(void*, void* a2, void* a3);
	typedef void* (__thiscall *SkillBuff_DebufTrap_GetResistancePtr)(void*);
	typedef void* (__thiscall *SkillBuff_DebufFreeze_GetResistancePtr)(void*);

	// void GAME::Character::GetAllDefenseAttributes(class GAME::CombatAttributeAccumulator &)
	typedef void* (__thiscall *Character_GetAllDefenseAttributesPtr)(void*, void*);

	static HANDLE m_hEvent;
	static CharacterTakeAttackPtr originalCharacterTakeAttackMethod;
	static GetTotalDefenseTypePtr originalGetTotalDefenseTypeMethod;

	static CharAttributeMod_TotalSpeed_AddToAccumulatorPtr originalCharAttributeMod_TotalSpeed_AddToAccumulatorMethod;
	static CharAttributeAccumulator_ExecuteDefensePtr originalCharAttributeAccumulator_ExecuteDefenseMethod;
	static SkillBuff_DebufTrap_GetResistancePtr originalSkillBuff_DebufTrap_GetResistanceMethod;
	static SkillBuff_DebufFreeze_GetResistancePtr originalSkillBuff_DebufFreeze_GetResistanceMethod;
	static Character_GetAllDefenseAttributesPtr originalCharacter_GetAllDefenseAttributesMethod;

	static GetObjectIdMethodPtr GetObjectId;

	static DataQueue* m_dataQueue;
	static int previousId;


	static void* __fastcall HookedCharacterTakeAttack(void* This, void* _, void* classParametersCombat);
	static float __fastcall HookedGetTotalDefenseType(void* This, void* _, float classCombatAttributeType);

	static void* __fastcall HookedCharAttributeMod_TotalSpeed_AddToAccumulator(void* This, void* _, void* a2, void* a3);
	static void* __fastcall HookedCharAttributeAccumulator_ExecuteDefense(void* This, void* _, void* a2, void* a3);
	static void* __fastcall HookedSkillBuff_DebufTrap_GetResistance(void* This, void* _);
	static void* __fastcall HookedSkillBuff_DebufFreeze_GetResistance(void* This, void* _);
	static void* __fastcall HookedCharacter_GetAllDefenseAttributes(void* This, void* _, void* arg1);
};
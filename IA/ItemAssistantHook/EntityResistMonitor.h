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
	typedef void* (__thiscall *CharacterTakeAttackPtr)(void*, void* classParametersCombat);
	//typedef void* (__thiscall CombatManagerTakeAttackPtr)(void*, void* classParametersCombat, void* classSkillManager, void* classCharacterBio);
	typedef float (__thiscall *GetTotalDefenseTypePtr)(void*, float classCombatAttributeType);

	typedef int (__thiscall *GetObjectIdMethodPtr)(void*);
	static HANDLE m_hEvent;
	static CharacterTakeAttackPtr originalCharacterTakeAttackMethod;
	static GetTotalDefenseTypePtr originalGetTotalDefenseTypeMethod;
	static GetObjectIdMethodPtr GetObjectId;
	static DataQueue* m_dataQueue;
	static int previousId;


	static void* __fastcall HookedCharacterTakeAttack(void* This, void* _, void* classParametersCombat);
	static float __fastcall HookedGetTotalDefenseType(void* This, void* _, float classCombatAttributeType);
	
};
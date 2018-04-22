#pragma once
#pragma once
#include <windows.h>
#include "DataQueue.h"
#include "BaseMethodHook.h"

class ApplyDamageHook : public BaseMethodHook {
public:
	ApplyDamageHook();
	ApplyDamageHook(DataQueue* dataQueue, HANDLE hEvent);
	void EnableHook();
	void DisableHook();


	//private:
	// void GAME::Character::ApplyDamage(float, struct GAME::PlayStatsDamageType const &, enum GAME::CombatAttributeType, class mem::vector<unsigned int> const &)
	typedef void* (__thiscall *OriginalMethodPtr)(void* This, float f, int* PlayStatsDamageType, int CombatAttributeType, void* Vector);
	static HANDLE m_hEvent;
	static OriginalMethodPtr originalMethod;
	static DataQueue* m_dataQueue;

	static void* __fastcall HookedMethod(void* This, void* notUsed, float f, int* PlayStatsDamageType, int CombatAttributeType, void* Vector);
};
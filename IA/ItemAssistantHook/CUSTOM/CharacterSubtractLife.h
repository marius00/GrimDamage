#pragma once
#include "..\BaseMethodHook.h"

class CharacterSubtractLife : public BaseMethodHook {
public:
	CharacterSubtractLife();
	CharacterSubtractLife(DataQueue* dataQueue, HANDLE hEvent);
	void EnableHook();
	void DisableHook();


//private:
	// void GAME::Character::SubtractLife(float,struct GAME::PlayStatsDamageType const &,bool,bool)
	typedef void* (__thiscall *OriginalMethodPtr)(void* This, float f, int* PlayStatsDamageType, bool a, bool b);
	static HANDLE m_hEvent;
	static OriginalMethodPtr originalMethod;
	static DataQueue* m_dataQueue;

	//static void* __fastcall HookedMethod(void* This, void* _, float f, int* PlayStatsDamageType, bool a, bool b);
};
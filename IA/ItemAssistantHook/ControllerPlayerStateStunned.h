#include <windows.h>
#include "DataQueue.h"
#include "BaseMethodHook.h"

/************************************************************************
/************************************************************************/
class ControllerPlayerStateStunned : public BaseMethodHook {
public:
	ControllerPlayerStateStunned();
	ControllerPlayerStateStunned(DataQueue* dataQueue, HANDLE hEvent);
	void EnableHook();
	void DisableHook();

private:
	typedef int* (__thiscall *MethodPtr)(void*);
	static HANDLE m_hEvent;
	static MethodPtr originalBegin;
	static MethodPtr originalEnd;
	static DataQueue* m_dataQueue;


	static void* __fastcall HookedBegin(void* This, void* notUsed);
	static void* __fastcall HookedEnd(void* This, void* notUsed);

};
#include <windows.h>
#include "DataQueue.h"
#include "BaseMethodHook.h"

/************************************************************************
/************************************************************************/
class HitpointMonitor : public BaseMethodHook {
public:
	HitpointMonitor();
	HitpointMonitor(DataQueue* dataQueue, HANDLE hEvent);
	void EnableHook();
	void DisableHook();

private:
	typedef void* (__thiscall *OriginalMethodPtr)(void*, int val);
	typedef int (__thiscall *GetObjectIdMethodPtr)(void*);
	static HANDLE m_hEvent;
	static OriginalMethodPtr originalMethod;
	static GetObjectIdMethodPtr GetObjectId;
	static DataQueue* m_dataQueue;


	static void* __fastcall HookedMethod(void* This, void* notUsed, int val);
	static bool DetectOffset();

	static int m_offset;
	
};
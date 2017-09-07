#include <windows.h>
#include "DataQueue.h"
#include "BaseMethodHook.h"

/************************************************************************
/************************************************************************/
class DetectPlayerId : public BaseMethodHook {
public:
	DetectPlayerId();
	DetectPlayerId(DataQueue* dataQueue, HANDLE hEvent);
	void EnableHook();
	void DisableHook();

private:
	struct Vec3f {
		float x,y,z,u;
	};

	typedef void* (__thiscall *OriginalMethodPtr)(void*);
	typedef int (__thiscall *GetObjectIdMethodPtr)(void*);
	static HANDLE m_hEvent;
	static OriginalMethodPtr originalMethod;
	static GetObjectIdMethodPtr GetObjectId;
	static DataQueue* m_dataQueue;


	static void* __fastcall HookedMethod(void* This, void* notUsed);
	
};
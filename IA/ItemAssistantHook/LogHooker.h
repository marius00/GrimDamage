#pragma once
#include <windows.h>
#include "DataQueue.h"
#include "BaseMethodHook.h"

class LoggerHook : public BaseMethodHook {
public:
	LoggerHook();
	LoggerHook(DataQueue* dataQueue, HANDLE hEvent);
	void EnableHook();
	void DisableHook();


private:
	typedef void (__cdecl *OriginalMethodPtr)(void* This, void* priority, char* str, void* args);
	static HANDLE m_hEvent;
	static OriginalMethodPtr originalMethod;
	static DataQueue* m_dataQueue;
	static MessageType m_message;

	static void __cdecl HookedMethod(
		void* This, 
		void* priority,
		char* dummytest, 
		char* str,
		void* _param0, 
		void* _param1,
		void* _param2,
		void* _param3,
		void* _param4,
		void* _param5
	);
};
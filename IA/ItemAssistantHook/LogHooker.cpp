#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include "MessageType.h"
#include "LogHooker.h"
#include "Globals.h"

HANDLE LoggerHook::m_hEvent;
DataQueue* LoggerHook::m_dataQueue;
LoggerHook::OriginalMethodPtr LoggerHook::originalMethod;
MessageType LoggerHook::m_message;

void LoggerHook::EnableHook() {
	// void GAME::Engine::Log(enum GAME::LogPriority,unsigned int,char const *,...) // most viable
	HookEngine("?Log@Engine@GAME@@UBAXW4LogPriority@2@IPBDZZ", HookedMethod, m_dataQueue, m_hEvent, Type_LogEvent);

	// void GAME::Engine::Log(enum GAME::LogPriority,char const *,...)
	//HookEngine("?Log@Engine@GAME@@UBAXW4LogPriority@2@PBDZZ", HookedMethod, m_dataQueue, m_hEvent, Type_LogEvent);
}

LoggerHook::LoggerHook(DataQueue* dataQueue, HANDLE hEvent) {
	m_dataQueue = dataQueue;
	m_hEvent = hEvent;
	m_message = Type_LogEvent;
}

LoggerHook::LoggerHook() {
	m_hEvent = NULL;
}

void LoggerHook::DisableHook() {
	Unhook(originalMethod, HookedMethod);
}

/*
   (*(void (**)(struct GAME::Engine *, _DWORD, const char *, ...))(*(_DWORD *)GAME::gEngine + 12))(
		GAME::gEngine,
		0,
		"%u epics and legendaries dropped this session",
		*((_DWORD *)this + 7536)
	);

	.text:101CE60B                   push    dword ptr [edi+75C0h]
	.text:101CE611                   mov     edx, [eax]
	.text:101CE613                   push    offset aUEpicsAndLegen ; "%u epics and legendaries dropped this s"...
	.text:101CE618                   push    0
	.text:101CE61A                   push    eax
	.text:101CE61B                   call    dword ptr [edx+0Ch]

 */
// ida pro says str is the 5th param
void __cdecl LoggerHook::HookedMethod(void* This, void* priority, char* str_, char* str, void* _param0, void* _param1, void* _param2, void* _param3, void* _param4, void* _param5) {
	{
		const size_t buffsize = 1 + PointerHelper::size;
		char buffer[buffsize] = { 0 };
		auto helper = readOffset(str);
		buffer[0] = 1;
		helper.toChar(buffer + 1);

		DataItemPtr item(new DataItem(m_message, buffsize, (char*)&buffer));
		m_dataQueue->push(item);
		SetEvent(m_hEvent);
	}
	{
		const size_t buffsize = 255;
		char buffer[buffsize] = { 0 };
		SIZE_T bytesRead = 0;
		HANDLE hProcess = GetCurrentProcess();

		const size_t resultSize = 4096;
		char result[resultSize] = {0};
		if (ReadProcessMemory(hProcess, str, (void*)&buffer, buffsize, &bytesRead) != 0) {

			sprintf_s(result, resultSize, buffer, _param0, _param1, _param2, _param3, _param4, _param5);
		}

		DataItemPtr item(new DataItem(45454, 4096, (char*)&result));
		m_dataQueue->push(item);
		SetEvent(m_hEvent);
	}

}
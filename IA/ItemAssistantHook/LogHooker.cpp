#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include "LogHooker.h"
#include "Globals.h"

HANDLE LoggerHook::m_hEvent;
DataQueue* LoggerHook::m_dataQueue;
LoggerHook::OriginalMethodPtr LoggerHook::originalMethod;

void LoggerHook::EnableHook() {
	// void GAME::Engine::Log(enum GAME::LogPriority,unsigned int,char const *,...) // most viable
	HookEngine("?Log@Engine@GAME@@UBAXW4LogPriority@2@IPBDZZ", HookedMethod, m_dataQueue, m_hEvent, 12345);

	// void GAME::Engine::Log(enum GAME::LogPriority,char const *,...)
	//HookEngine("?Log@Engine@GAME@@UBAXW4LogPriority@2@PBDZZ", HookedMethod, m_dataQueue, m_hEvent, Type_LogEvent);
}

LoggerHook::LoggerHook(DataQueue* dataQueue, HANDLE hEvent) {
	m_dataQueue = dataQueue;
	m_hEvent = hEvent;
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

const char* MSG_DAMAGE_TO_DEFENDER = "^y    Damage %f to Defender 0x%x (%s)"; // 3
const char* MSG_LIFE_LEECH = "    ^str%f%% Life Leech return %f Life"; // 2
const char* MSG_TOTALDMG = "    Total Damage:  Absolute (%f), Over Time (%f)"; //2
const char* MSG_ATTACKER_NAME = "    attackerName = %s"; // 1
const char* MSG_ATTACKER_ID = "    attackerID = %d"; // 1
const char* MSG_DEFENDER_NAME = "    defenderName = %s"; // 1
const char* MSG_DEFENDER_ID = "    defenderID = %d"; // 1
const char* MSG_DEFLECT = "    ^yDeflect Projectile Chance (%f) caused prefix deflection"; //1
const char* MSG_ABSORB = "    protectionAbsorption = %f"; //1
const char* MSG_REFLECT = "    ^str%f%% Damage Reflected"; //1
const char* MSG_BLOCK = "^bShield: Reduced (%f) Damage by (%f%) percent, remaining damage (%f)"; //3


bool startsWith(const char* prefix, char* str) {
	size_t lenpre = strlen(prefix), lenstr = strlen(str);
	return lenstr < lenpre ? false : strncmp(prefix, str, lenpre) == 0;
}

// ida pro says str is the 5th param
void __cdecl LoggerHook::HookedMethod(
	void* This, 
	void* priority, 
	char* str_, 
	char* str, 
	void* _param0, 
	void* _param1, 
	void* _param2,
	void* _param3,
	void* _param4,
	void* _param5) {
/*
	{
		const size_t buffsize = 1 + PointerHelper::size;
		char buffer[buffsize] = { 0 };
		auto helper = readOffset(str);
		buffer[0] = 1;
		helper.toChar(buffer + 1);

		DataItemPtr item(new DataItem(m_message, buffsize, (char*)&buffer));
		m_dataQueue->push(item);
		SetEvent(m_hEvent);
	}*/
	{
		const size_t buffsize = 1000;
		char logMessage[buffsize] = { 0 };
		SIZE_T bytesRead = 0;
		HANDLE hProcess = GetCurrentProcess();

		if (ReadProcessMemory(hProcess, str, (void*)&logMessage, buffsize, &bytesRead) != 0) {

			if (startsWith(MSG_DAMAGE_TO_DEFENDER, logMessage)) {
				int messageId = 45001;
				const size_t resultSize = 256 + 8*2 + 4;
				char result[resultSize] = { 0 };

				// f x s
				if (
					ReadProcessMemory(hProcess, &_param0, (void*)&result, 4, &bytesRead) != 0 // %f
					&& ReadProcessMemory(hProcess, &_param1, (char*)&result + 4, 4, &bytesRead) != 0 // %f

					&& ReadProcessMemory(hProcess, &_param2, (char*)&result + 8, 4, &bytesRead) != 0 // %x

					&& ReadProcessMemory(hProcess, _param3, (char*)&result + 12, 255, &bytesRead) != 0) {  // %s

					DataItemPtr item(new DataItem(messageId, resultSize, (char*)&result));
					m_dataQueue->push(item);
					SetEvent(m_hEvent);
				}
			}
			else if (startsWith(MSG_LIFE_LEECH, logMessage)) {
				int messageId = 45002;
				const size_t resultSize = 16;
				char result[resultSize] = { 0 };
				if (
					ReadProcessMemory(hProcess, &_param0, (void*)&result, 4, &bytesRead) != 0
					&& ReadProcessMemory(hProcess, &_param1, (char*)&result + 4, 4, &bytesRead) != 0
					&& ReadProcessMemory(hProcess, &_param2, (char*)&result + 8, 4, &bytesRead) != 0
					&& ReadProcessMemory(hProcess, &_param3, (char*)&result + 12, 4, &bytesRead) != 0
					) {
					DataItemPtr item(new DataItem(messageId, resultSize, (char*)&result));
					m_dataQueue->push(item);
					SetEvent(m_hEvent);
				}
				else {
					DataItemPtr item(new DataItem(messageId, 0, nullptr));
					m_dataQueue->push(item);
					SetEvent(m_hEvent);
				}
			}
			else if (startsWith(MSG_TOTALDMG, logMessage)) {
				int messageId = 45003;
				const size_t resultSize = 16;
				char result[resultSize] = { 0 };
				if (
					ReadProcessMemory(hProcess, &_param0, (void*)&result, 4, &bytesRead) != 0
					&& ReadProcessMemory(hProcess, &_param1, (char*)&result + 4, 4, &bytesRead) != 0
					&& ReadProcessMemory(hProcess, &_param2, (char*)&result + 8, 4, &bytesRead) != 0
					&& ReadProcessMemory(hProcess, &_param3, (char*)&result + 12, 4, &bytesRead) != 0
					) {
					DataItemPtr item(new DataItem(messageId, resultSize, (char*)&result));
					m_dataQueue->push(item);
					SetEvent(m_hEvent);
				}
				else {
					DataItemPtr item(new DataItem(messageId, 0, nullptr));
					m_dataQueue->push(item);
					SetEvent(m_hEvent);
				}
			}
			else if (startsWith(MSG_ATTACKER_NAME, logMessage)) {
				const size_t resultSize = 256;
				char result[resultSize] = { 0 };
				if (ReadProcessMemory(hProcess, _param0, (void*)&result, 255, &bytesRead) != 0) {
					DataItemPtr item(new DataItem(45004, resultSize, (char*)&result));
					m_dataQueue->push(item);
					SetEvent(m_hEvent);
				} else {
					DataItemPtr item(new DataItem(45004, 0, nullptr));
					m_dataQueue->push(item);
					SetEvent(m_hEvent);
				}
			}
			else if (startsWith(MSG_DEFENDER_NAME, logMessage)) {
				const size_t resultSize = 256;
				char result[resultSize] = { 0 };

				if (ReadProcessMemory(hProcess, _param0, (void*)&result, 255, &bytesRead) != 0) {
					DataItemPtr item(new DataItem(45005, resultSize, (char*)&result));
					m_dataQueue->push(item);
					SetEvent(m_hEvent);
				}
				else {
					DataItemPtr item(new DataItem(45005, 0, nullptr));
					m_dataQueue->push(item);
					SetEvent(m_hEvent);
				}
			}
			else if (startsWith(MSG_ATTACKER_ID, logMessage)) {
				const size_t resultSize = 4;
				char result[resultSize] = { 0 };
				memcpy(&result, &_param0, 4);

				DataItemPtr item(new DataItem(45006, resultSize, (char*)&result));
				m_dataQueue->push(item);
				SetEvent(m_hEvent);
			}
			else if (startsWith(MSG_DEFENDER_ID, logMessage)) {
				const size_t resultSize = 4;
				char result[resultSize] = { 0 };
				memcpy(&result, &_param0, 4);

				DataItemPtr item(new DataItem(45007, resultSize, (char*)&result));
				m_dataQueue->push(item);
				SetEvent(m_hEvent);
			}
			else if (startsWith(MSG_DEFLECT, logMessage)) {
				int messageId = 45008;
				const size_t resultSize = 8;
				char result[resultSize] = { 0 };
				if (
					ReadProcessMemory(hProcess, &_param0, (void*)&result, 4, &bytesRead) != 0
					&& ReadProcessMemory(hProcess, &_param1, (char*)&result + 4, 4, &bytesRead) != 0
					) {
					DataItemPtr item(new DataItem(messageId, resultSize, (char*)&result));
					m_dataQueue->push(item);
					SetEvent(m_hEvent);
				}
				else {
					DataItemPtr item(new DataItem(messageId, 0, nullptr));
					m_dataQueue->push(item);
					SetEvent(m_hEvent);
				}
			}
			else if (startsWith(MSG_ABSORB, logMessage)) {
				const size_t resultSize = 8;
				char result[resultSize] = { 0 };
				if (
					ReadProcessMemory(hProcess, &_param0, (void*)&result, 4, &bytesRead) != 0
					&& ReadProcessMemory(hProcess, &_param1, (char*)&result+4, 4, &bytesRead) != 0
				) {
					DataItemPtr item(new DataItem(45009, resultSize, (char*)&result));
					m_dataQueue->push(item);
					SetEvent(m_hEvent);
				}
				else {
					DataItemPtr item(new DataItem(45009, 0, nullptr));
					m_dataQueue->push(item);
					SetEvent(m_hEvent);
				}
			}
			else if (startsWith(MSG_REFLECT, logMessage)) {
				const size_t resultSize = 8;
				char result[resultSize] = { 0 };
				if (
					ReadProcessMemory(hProcess, &_param0, (void*)&result, 4, &bytesRead) != 0
					&& ReadProcessMemory(hProcess, &_param1, (char*)&result + 4, 4, &bytesRead) != 0
					) {
					DataItemPtr item(new DataItem(45010, resultSize, (char*)&result));
					m_dataQueue->push(item);
					SetEvent(m_hEvent);
				}
				else {
					DataItemPtr item(new DataItem(45010, 0, nullptr));
					m_dataQueue->push(item);
					SetEvent(m_hEvent);
				}
			}
			else if (startsWith(MSG_BLOCK, logMessage)) {
				int messageId = 45011;
				const size_t resultSize = 8*3;
				char result[resultSize] = { 0 };
				if (
					ReadProcessMemory(hProcess, &_param0, (void*)&result, 4, &bytesRead) != 0
					&& ReadProcessMemory(hProcess, &_param1, (char*)&result + 4, 4, &bytesRead) != 0

					&& ReadProcessMemory(hProcess, &_param2, (char*)&result + 8, 4, &bytesRead) != 0
					&& ReadProcessMemory(hProcess, &_param3, (char*)&result + 12, 4, &bytesRead) != 0

					&& ReadProcessMemory(hProcess, &_param4, (char*)&result + 16, 4, &bytesRead) != 0
					&& ReadProcessMemory(hProcess, &_param5, (char*)&result + 20, 4, &bytesRead) != 0
					) {
					DataItemPtr item(new DataItem(messageId, resultSize, (char*)&result));
					m_dataQueue->push(item);
					SetEvent(m_hEvent);
				}
				else {
					DataItemPtr item(new DataItem(messageId, 0, nullptr));
					m_dataQueue->push(item);
					SetEvent(m_hEvent);
				}
			}
			else {
				DataItemPtr item(new DataItem(45000, buffsize, (char*)&logMessage));
				m_dataQueue->push(item);
				SetEvent(m_hEvent);
			}
			/*
			if (isRelevant(logMessage)) {
				sprintf_s(result, resultSize, logMessage, _param0, _param1, _param2);
				DataItemPtr item(new DataItem(45001, resultSize, (char*)&result));
				m_dataQueue->push(item);
				SetEvent(m_hEvent);
			}
			else {
				DataItemPtr item(new DataItem(45001, buffsize, (char*)&logMessage));
				m_dataQueue->push(item);
				SetEvent(m_hEvent);
				
			}*/

		}

	}

}
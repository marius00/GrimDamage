#include "stdafx.h"
#include <shared/aopackets.h>
#include <windows.h>
#include <detours.h>
#include <stdio.h>
#include <stdlib.h>
#include <set>
#include <boost/thread.hpp>
#include "DataQueue.h"
#include "Globals.h"
#include "LogHooker.h"
#include "ControllerPlayerStateMoveToRequestNpcAction.h"
#include "ControllerPlayerStateMoveToRequestMoveAction.h"
#include "HookWalkTo.h"
#include "ControllerPlayerStateIdleRequestNpcAction.h"
#include "ControllerPlayerStateIdleRequestInteractableAction.h"
#include "PauseGameTime.h"
#include "UnpauseGameTime.h"
#include "DetectPlayerId.h"
#include "IncrementDeaths.h"


#pragma region Variables
// Switches hook logging on/off
#if 1
#include "HookLog.h"
HookLog g_log;
#define LOG(streamdef) \
{ \
    std::string msg = (((std::ostringstream&)(std::ostringstream().flush() << streamdef)).str()); \
    g_log.out(msg); \
    msg += _T("\n"); \
    OutputDebugString(msg.c_str()); \
}
#else
#define LOG(streamdef) \
    __noop;
#endif


PVOID Func;

DWORD g_lastTick = 0;
DWORD g_lastThreadTick = 0;
std::set<unsigned int> g_messageFilter;
HANDLE g_hEvent;
HANDLE g_thread;

DataQueue g_dataQueue;

HWND g_targetWnd = NULL;   // TODO: Make this a set/list that is dynamically updated

using namespace AO;

PDETOUR_TRAMPOLINE Trampoline = NULL;
#pragma endregion

#pragma region CORE


/// Thread function that dispatches queued message blocks to the AOIA application.
void WorkerThreadMethod() {
    while ((g_hEvent != NULL) && (WaitForSingleObject(g_hEvent,INFINITE) == WAIT_OBJECT_0)) {
        if (g_hEvent == NULL) {
            break;
        }

        DWORD tick = GetTickCount();
        if (tick < g_lastThreadTick) {
            // Overflow
            g_lastThreadTick = tick;
        }

        if ((tick - g_lastThreadTick > 1000) || (g_targetWnd == NULL)) {
            // We either don't have a valid window target OR it has been more than 1 sec since we last update the target.
            g_targetWnd = FindWindow( "GDDamageWindowClass", NULL);
            g_lastThreadTick = GetTickCount();
            LOG("FindWindow returned: " << g_targetWnd);
        }

        while (!g_dataQueue.empty()) {
            DataItemPtr item = g_dataQueue.pop();

            if (g_targetWnd == NULL) {
                // We have data, but no target window, so just delete the message
                continue;
            }

            COPYDATASTRUCT data;
            data.dwData = item->type();
            data.lpData = item->data();
            data.cbData = item->size();

            // To avoid blocking the main thread, we should not have a lock on the queue while we process the message.
			SendMessage( g_targetWnd, WM_COPYDATA, 0, ( LPARAM ) &data );
            LOG("After SendMessage error code is " << GetLastError());
        }
    }
}

unsigned __stdcall WorkerThreadMethodWrap(void* argss) {
	WorkerThreadMethod();
	return 0;
}
void StartWorkerThread() {

	unsigned int pid;
	//g_thread = (HANDLE)_beginthread(WorkerThreadMethod,0,0);
	g_thread = (HANDLE)_beginthreadex(NULL, 0, &WorkerThreadMethodWrap, NULL, 0, &pid);
}


void EndWorkerThread() {
	if (g_hEvent != NULL) {
		SetEvent(g_hEvent);
		HANDLE h = g_hEvent;
		g_hEvent = NULL;
		CloseHandle(h);

		//WaitForSingleObject(g_thread, INFINITE);
		CloseHandle(g_thread);
	}
}

#pragma endregion

std::vector<BaseMethodHook*> hooks;
int ProcessAttach(HINSTANCE _hModule) {
	g_hEvent = CreateEvent(NULL,FALSE,FALSE,"IA_Worker");

	hooks.push_back(new ControllerPlayerStateIdleRequestInteractableAction(&g_dataQueue, g_hEvent));
	hooks.push_back(new ControllerPlayerStateIdleRequestNpcAction(&g_dataQueue, g_hEvent));
	hooks.push_back(new ControllerPlayerStateMoveToRequestNpcAction(&g_dataQueue, g_hEvent));
	hooks.push_back(new ControllerPlayerStateMoveToRequestMoveAction(&g_dataQueue, g_hEvent));
	hooks.push_back(new HookWalkTo(&g_dataQueue, g_hEvent));

	hooks.push_back(new PauseGameTime(&g_dataQueue, g_hEvent));
	hooks.push_back(new UnpauseGameTime(&g_dataQueue, g_hEvent));
	hooks.push_back(new LoggerHook(&g_dataQueue, g_hEvent));
	//hooks.push_back(new DetectPlayerId(&g_dataQueue, g_hEvent));
	hooks.push_back(new IncrementDeaths(&g_dataQueue, g_hEvent));
	
	
	for (unsigned int i = 0; i < hooks.size(); i++) {
		hooks[i]->EnableHook();
	}

	
    StartWorkerThread();
    return TRUE;
}

__declspec(dllexport) void Test(char*) {
}


#pragma region Attach_Detatch
int ProcessDetach( HINSTANCE _hModule ) {
	// Signal that we are shutting down
	// This message is not at all guaranteed to get sent.
	char b[1]{ 0 };
	DataItemPtr dataEvent(new DataItem(TYPE_HookUnload, 1, (char*)b));
	g_dataQueue.push(dataEvent);
	SetEvent(g_hEvent);


	OutputDebugString("ProcessDetach");


	for (unsigned int i = 0; i < hooks.size(); i++) {
		hooks[i]->DisableHook();
		delete hooks[i];
	}
	hooks.clear();


    EndWorkerThread();

    return TRUE;
}


BOOL APIENTRY DllMain(HINSTANCE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved) {
    switch (ul_reason_for_call) {
	case DLL_PROCESS_ATTACH:
        return ProcessAttach( hModule );

	case DLL_PROCESS_DETACH:
        return ProcessDetach( hModule );

	case DLL_THREAD_ATTACH:
        break;

	case DLL_THREAD_DETACH:
        break;
	}
    return TRUE;
}
#pragma endregion


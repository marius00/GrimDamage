#pragma once
//#define REPLICA_PTR_OFFSET 832

struct PointerHelper {
	int address;
	bool success;
	int errorCode;
	int value;

	bool isPointerToPointer;
	int ptrToPtrValue;

	static const size_t size = sizeof(int) * 4 + sizeof(bool) * 2;
	
	void toChar(char* dst) {
		memcpy(dst+0, &address, 4);
		memcpy(dst+4, &errorCode, 4);
		memcpy(dst+8, &value, 4);
		memcpy(dst + 12, &ptrToPtrValue, 4);
		memcpy(dst + 16, &isPointerToPointer, 4);
		memcpy(dst + 17, &success, 4);
	}
};


int CopyGDString(char* srcObj, char* buffer, size_t bufsize);
PointerHelper readOffset(void* ptr);

#define TYPE_WALKTO 1001
#define TYPE_ControllerPlayerStateIdleRequestNpcAction 1002
#define TYPE_ControllerPlayerStateIdleRequestInteractableAction 1003
#define TYPE_ControllerPlayerStateMoveToRequestMoveAction 1004
#define TYPE_ControllerPlayerStateMoveToRequestNpcAction 1005
#define TYPE_ControllerPlayerStateIdleRequestMoveAction 1006
#define TYPE_ERROR_HOOKING_GENERIC 1
#define TYPE_HookUnload 3
#define TYPE_PauseGameTime 20000
#define TYPE_UnpauseGameTime 20001
#define TYPE_DetectPlayerId 20002
#define TYPE_IncrementDeaths 20003
#define TYPE_StunBegin 2001
#define TYPE_StunEnd 2002
#define TYPE_TrapBegin 2003
#define TYPE_TrapEnd 2004
#define TYPE_DisableMovement 2005
#define TYPE_SetLifeState 2006
#define TYPE_PlayerHealthOffsetDetected 100
#define TYPE_ErrorDetectingPlayerHealthOffset 404
#define TYPE_HitpointMonitor 1007

#define TYPE_LOG_EndCombat 45012

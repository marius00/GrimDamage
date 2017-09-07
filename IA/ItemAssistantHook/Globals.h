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

#define TYPE_WALKTO 1000
#define TYPE_ControllerPlayerStateIdleRequestNpcAction 1000
#define TYPE_ControllerPlayerStateIdleRequestInteractableAction 1000
#define TYPE_ControllerPlayerStateMoveToRequestMoveAction 1000
#define TYPE_ControllerPlayerStateMoveToRequestNpcAction 1000
#define TYPE_ControllerPlayerStateIdleRequestMoveAction 1000
#define TYPE_ERROR_HOOKING_GENERIC 1
#define TYPE_HookUnload 3
#define TYPE_PauseGameTime 20000
#define TYPE_UnpauseGameTime 20001
#define TYPE_DetectPlayerId 20002
#define TYPE_IncrementDeaths 20003
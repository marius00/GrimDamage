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


int FindOffset();
int CopyGDString(char* srcObj, char* buffer, size_t bufsize);
PointerHelper readOffset(void* ptr);
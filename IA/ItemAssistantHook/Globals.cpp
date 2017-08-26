#include <windows.h>
#include "Globals.h"


PointerHelper readOffset(void* ptr) {
	SIZE_T bytesRead = 0;
	const size_t scanRange = 4;
	HANDLE hProcess = GetCurrentProcess();

	PointerHelper result;
	result.success = ReadProcessMemory(hProcess, ptr, (void*)&result.value, scanRange, &bytesRead) != 0;
	if (!result.success) {
		result.errorCode = GetLastError();
		result.isPointerToPointer = false;
		result.ptrToPtrValue = 0;
	}
	else {
		result.isPointerToPointer = ReadProcessMemory(hProcess, (void*)result.value, (void*)&result.ptrToPtrValue, scanRange, &bytesRead) != 0;
		result.errorCode = 0;
	}

	result.address = (int)ptr;

	return result;
}
#include <Windows.h>
#include "IComLogger.h"

class ComLogger : IComLogger
{
	private: ULONG refCount;

	public: ComLogger();
	
	public: HRESULT __stdcall QueryInterface(REFIID guid, LPVOID* interfacePointer);	
	public: ULONG __stdcall AddRef();	
	public: ULONG __stdcall Release();
	
	public: void __stdcall Log(BSTR message);
};
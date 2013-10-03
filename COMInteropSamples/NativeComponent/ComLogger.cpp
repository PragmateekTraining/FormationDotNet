#include <locale>
#include <Windows.h>
#include <ios>
#include <fstream>
#include <string.h>
#include "ComLogger.h"

ComLogger::ComLogger()
{
	this->refCount = 0;
}
	
HRESULT __stdcall ComLogger::QueryInterface(REFIID iid, LPVOID* interfacePointer)
{
	*interfacePointer = NULL;
	if ((iid == IID_IUnknown) || (iid == IID_IComLogger))
	{	
		*interfacePointer = this;
		AddRef();
	}	
	else
	{
		return E_NOINTERFACE;
	}
	
	return S_OK;
}

ULONG __stdcall ComLogger::AddRef()
{
	return ::InterlockedIncrement(&refCount);
}

ULONG __stdcall ComLogger::Release()
{
	ULONG newRefCount = ::InterlockedDecrement(&refCount);

	if (newRefCount == 0)
	{
		delete this;
	}
	
	return refCount;
}

void __stdcall ComLogger::Log(BSTR message)
{
	std::wofstream log("C:\\tmp\\logs.txt", std::ios_base::app | std::ios_base::out);

	std::wstring ws(message);
	
	log << ws << std::endl;
}
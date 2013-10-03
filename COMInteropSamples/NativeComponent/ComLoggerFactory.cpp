#include <Windows.h>
#include "ComLoggerFactory.h"
#include "ComLogger.h"

extern char* ToString(const GUID & guid);

ComLoggerFactory::ComLoggerFactory()
{
	this->refCount = 0;
}

HRESULT __stdcall ComLoggerFactory::QueryInterface(REFIID iid, LPVOID* interfacePointer)
{
	*interfacePointer = NULL;
	if ((iid == IID_IUnknown) || (iid == IID_IClassFactory))
	{	
		*interfacePointer = this;
		AddRef();
	}
	else
	{
		return E_NOINTERFACE;
	}
	
	return NOERROR;
}

ULONG __stdcall ComLoggerFactory::AddRef()
{
	return ::InterlockedIncrement(&refCount);
}
	
ULONG __stdcall ComLoggerFactory::Release()
{
	ULONG newRefCount = ::InterlockedDecrement(&refCount);

	if (newRefCount == 0)
	{
		delete this;
	}
	
	return newRefCount;
}

HRESULT __stdcall ComLoggerFactory::LockServer(BOOL bLock)
{
    return S_OK ;
}

HRESULT __stdcall ComLoggerFactory::CreateInstance(IUnknown* pUnkOuter, REFIID iid, LPVOID* interfacePointer)
{
	if (pUnkOuter != NULL)
	{
		return CLASS_E_NOAGGREGATION;
	}
	
	*interfacePointer = NULL;

	ComLogger* logger = new ComLogger();
	logger->AddRef();
	
	HRESULT hr = logger->QueryInterface(iid, interfacePointer);
	
	logger->Release();

	return hr;
}
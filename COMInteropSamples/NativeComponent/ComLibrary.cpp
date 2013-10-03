#include <Windows.h>
#include "ComLoggerFactory.h"
#include "IComLogger.h"

STDAPI DllGetClassObject(REFCLSID clsid, REFIID iid, LPVOID* outInterfacePointer)
{
	if (clsid != CLSID_ComLogger)
	{
		return CLASS_E_CLASSNOTAVAILABLE;
	}
	
	ComLoggerFactory* factory = new ComLoggerFactory();
	factory->AddRef();

	*outInterfacePointer = NULL;	
	HRESULT hr = factory->QueryInterface(iid, outInterfacePointer);
	
	factory->Release();
	
	return hr;
}
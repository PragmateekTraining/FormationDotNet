#include <Windows.h>

class ComLoggerFactory : public IClassFactory
{
	private: ULONG refCount;
	
	public: ComLoggerFactory();
	
	public: HRESULT __stdcall QueryInterface(REFIID guid, LPVOID* interfacePointer);	
	public: ULONG __stdcall AddRef();	
	public: ULONG __stdcall Release();
	
	public: HRESULT __stdcall CreateInstance(IUnknown* pUnkOuter, REFIID iid, LPVOID* ppv);
	public: HRESULT __stdcall LockServer(BOOL bLock);
};
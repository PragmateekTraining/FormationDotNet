#include <Windows.h>
#include <string>
#include <iostream>
#include "IComLogger.h"

int main(int argc, char* argv[])
{	
	if (argc != 2)
	{
		std::cout << "No message to log!" << std::endl;
		return EXIT_FAILURE;
	}

	const size_t size = strlen(argv[1]) + 1;
    wchar_t* wc = new wchar_t[size];
    mbstowcs(wc, argv[1], size);
	
	BSTR message = SysAllocString(wc);
	
	::CoInitialize(NULL);

	IComLogger* logger = NULL;
	HRESULT hr = ::CoCreateInstance(CLSID_ComLogger, NULL, CLSCTX_INPROC_SERVER, IID_IComLogger, (LPVOID*)&logger);
	
	logger->Log(message);
	
	SysFreeString(message);
	
	logger->Release();
	
	::CoUninitialize();
}
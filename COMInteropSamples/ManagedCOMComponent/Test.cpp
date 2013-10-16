//#include "stdafx.h"
#import "ManagedCOMLogger.tlb"
#include <comutil.h>

int main()
{
	CoInitialize(NULL);

	COMInteropSamples::ILoggerPtr logger;// = new COMInteropSamples::Logger();
	HRESULT res = logger.CreateInstance(__uuidof(COMInteropSamples::Logger));
	if(FAILED(res))
        printf("KO with error 0x%08lx\n", res);
    else
    {
        printf("OK\n");
		logger->Path = "native_logs.log";
		logger->Log("Logging from native C++");
		// logger->F();
    }
	//logger->Path = "native_logs.log";
	// logger->Log("Logging from native C++");

	CoUninitialize();

	return 0;
}

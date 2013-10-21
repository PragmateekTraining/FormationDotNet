#include <cstdlib>
#include <iostream>
#include <metahost.h>
//#include <mscoree.h>
#pragma comment(lib, "mscoree.lib")

int wmain(int argc, wchar_t *argv[], wchar_t *envp[])
{
	if (argc < 2)
	{
		std::cerr << "No sample name provided!" << std::endl;
		return 1;
	}

	printf("Preparing host for sample '%s'\n", argv[1]);

	HRESULT hr;
	ICLRMetaHost* metaHost = NULL;	
	hr = CLRCreateInstance(CLSID_CLRMetaHost, IID_ICLRMetaHost, (LPVOID*)&metaHost);

	if (FAILED(hr))
	{
		std::cerr << "Unable to create meta-host!" << std::endl;
		return 1;
	}

	ICLRRuntimeInfo* runtimeInfo = NULL;
	hr = metaHost->GetRuntime(L"v4.0.30319", IID_ICLRRuntimeInfo, (LPVOID*)&runtimeInfo);

	if (FAILED(hr))
	{
		std::cerr << "Unable to get runtime!" << std::endl;
		return 1;
	}

	ICLRRuntimeHost* runtimeHost  = NULL;
	hr = runtimeInfo->GetInterface(CLSID_CLRRuntimeHost, IID_ICLRRuntimeHost, (LPVOID*)&runtimeHost);

	if (FAILED(hr))
	{
		std::cerr << "Unable to get runtime host!" << std::endl;
		return 1;
	}

	ICLRControl* clrControl = NULL;
	hr = runtimeHost->GetCLRControl(&clrControl);

	ICLRPolicyManager* clrPolicyManager = NULL;
	clrControl->GetCLRManager(IID_ICLRPolicyManager, (LPVOID*)&clrPolicyManager);

	// clrPolicyManager->SetDefaultAction(OPR_ThreadAbort, eRudeAbortThread);
	clrPolicyManager->SetTimeoutAndAction(OPR_ThreadAbort, 1000, eRudeAbortThread);

	hr = runtimeHost->Start();

	DWORD returnVal = 0;
	hr = runtimeHost->ExecuteInDefaultAppDomain(L"CERSamples.exe", L"CERSamples.EntryPoint", L"Call", argv[1], &returnVal);

	if (hr != S_OK)
	{
		std::cerr << "Unable to execute sample!" << std::endl;
		return 1;
	}

	hr = runtimeHost->Stop();

	return 0;
}
#include <windows.h>

typedef void (* log_delegate)(const char*);
typedef void (* progress_delegate)(double);

extern "C" __declspec(dllexport) int compute(int n, progress_delegate progress, log_delegate log)
{
	int fn_1 = 0;
	int fn = 1;

	log("Starting computation...");

	for (int i = 2; i <= n; ++i)
	{
		fn = fn + fn_1;
		fn_1 = fn - fn_1;

		progress(1.0 * i / n);

		Sleep(100);
	}

	log("Done.");

	return fn;
}
#include <stdio.h>

__declspec(dllexport) void print(const char* message)
{
	printf("%s\n", message);
}
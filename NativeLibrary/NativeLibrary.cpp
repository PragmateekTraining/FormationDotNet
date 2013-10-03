#include <stdint.h>
#include <stdio.h>

typedef union
{
	struct
	{
		int8_t LSB;
		int8_t MSB;
	};
	int16_t Value;
} MyShort;

extern "C"
{
	__declspec(dllexport) int super_fast_add(int a, int b)
	{
		return a + b;
	}

	__declspec(dllexport) void dump(MyShort myShort)
	{
		printf("{ [%d, %d], %d }\n", myShort.MSB, myShort.LSB, myShort.Value);
	}
}
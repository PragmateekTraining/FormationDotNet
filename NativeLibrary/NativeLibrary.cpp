#include <stdint.h>
#include <stdio.h>

typedef union
{
	struct
	{
		int8_t FirstByte;
		int8_t SecondByte;
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
		printf("{ [%d, %d], %d }\n", myShort.FirstByte, myShort.SecondByte, myShort.Value);
	}
}
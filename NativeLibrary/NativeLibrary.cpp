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

	static int* data;

	__declspec(dllexport) void set_data(int* p)
	{
		data = p;
	}

	__declspec(dllexport) int get_data()
	{
		return *data;
	}

	__declspec(dllexport) unsigned int sum(size_t size, const unsigned int* data)
	{
		unsigned int sum = 0;

		for (size_t i = 0; i < size; ++i)
		{
			sum += data[i];
		}

		return sum;
	}
}
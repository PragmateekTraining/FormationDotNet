#include <iostream>

extern "C"
{
	int Add(int, int);
	void Log(const char*);
}

int main()
{
	std::cout << Add(123, 456) << std::endl;
	
	Log("Hello from native C++.");

	return 0;
}
__declspec(dllexport) void spin()
{
	while (1);
}

__declspec(dllexport) void segfault()
{
	*(int*)0 = 1;
}
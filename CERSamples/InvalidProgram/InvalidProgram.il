.assembly InvalidProgram {}

.assembly extern mscorlib {}

.class public A
{
	.method public static void Crash()
	{
		ldarg.0
	}
}

.method static void Main()
{
	.entrypoint

	call void A::Crash()

	ret
}
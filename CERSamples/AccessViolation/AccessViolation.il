.assembly AccessViolation {}

.assembly extern mscorlib {}

.class public AccessViolation
{
	.method public static void Crash()
	{
		call void [mscorlib]System.Console::WriteLine(class [mscorlib]System.String)
	}
}

.method static void Main()
{
	.entrypoint

	call void AccessViolation::Crash()

	ret
}
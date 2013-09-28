.assembly Test {}

.assembly extern mscorlib {}

.method static valuetype [mscorlib]System.Byte F()
{
	ldstr "System.Byte F()"
	call void [mscorlib]System.Console::WriteLine(class [mscorlib]System.String)
	
	ldc.i4.0
	
	ret
}

.method static valuetype [mscorlib]System.SByte F()
{
	ldstr "System.SByte F()"
	call void [mscorlib]System.Console::WriteLine(class [mscorlib]System.String)
	
	ldc.i4.0
	
	ret
}

.method static void Main()
{
	.entrypoint

	call valuetype [mscorlib]System.Byte F()
	call valuetype [mscorlib]System.SByte F()

	ret
}
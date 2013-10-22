.assembly GCGenerationsSample { }
.assembly extern mscorlib { }

.class A extends [mscorlib]System.Object
{
	.field int32 n;

	.method instance void .ctor()
	{
		ldarg.0
		call instance void [mscorlib]System.Object::.ctor()
		ret
	}
}

.method static void Main()
{
	.entrypoint

	.locals (class A)

	newobj instance void A::.ctor()
	stloc.0

	// Display address of field "n"
	ldloc.0
	ldflda int32 A::n
	call void [mscorlib]System.Console::WriteLine(int32)

	// Display generation of object
	ldloc.0
	call int32 [mscorlib]System.GC::GetGeneration(object)
	call void [mscorlib]System.Console::WriteLine(int32)

	// Run a full GC
	call void [mscorlib]System.GC::Collect()

	// Display address of field "n"
	ldloc.0
	ldflda int32 A::n
	call void [mscorlib]System.Console::WriteLine(int32)

	// Display generation of object
	ldloc.0
	call int32 [mscorlib]System.GC::GetGeneration(object)
	call void [mscorlib]System.Console::WriteLine(int32)

	// Run a full GC
	call void [mscorlib]System.GC::Collect()

	// Display address of field "n"
	ldloc.0
	ldflda int32 A::n
	call void [mscorlib]System.Console::WriteLine(int32)

	// Display generation of object
	ldloc.0
	call int32 [mscorlib]System.GC::GetGeneration(object)
	call void [mscorlib]System.Console::WriteLine(int32)

	// Run a full GC
	call void [mscorlib]System.GC::Collect()

	// Display address of field "n"
	ldloc.0
	ldflda int32 A::n
	call void [mscorlib]System.Console::WriteLine(int32)

	// Display generation of object
	ldloc.0
	call int32 [mscorlib]System.GC::GetGeneration(object)
	call void [mscorlib]System.Console::WriteLine(int32)

	ret
}
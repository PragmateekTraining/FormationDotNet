.assembly Test{}

.assembly extern mscorlib{}

.class S extends [mscorlib]System.ValueType
{
    .field public int32 n;
}

.method static void F()
{
    .locals (int32, valuetype S)

    ldloc 0
    call void [mscorlib]System.Console::WriteLine(int32)

    ldloca 1
    ldfld int32 S::n
    call void [mscorlib]System.Console::WriteLine(int32)

    ret
}

.method static void Main()
{
    .entrypoint

    .locals (int32, valuetype S)

    ldloc 0
    call void [mscorlib]System.Console::WriteLine(int32)

    ldloca 1
    ldfld int32 S::n
    call void [mscorlib]System.Console::WriteLine(int32)

    call void F()

    ret
}
.assembly HelloWorld { }

.assembly extern mscorlib { }

.method static void Main()
{
        .entrypoint

		ldstr "We're in CIL!"
        call void [mscorlib]System.Console::WriteLine(string)

        ret
}
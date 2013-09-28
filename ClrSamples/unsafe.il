.assembly Unsafe{}

.method static void Main()
{
	.entrypoint

	.locals init(string, object)
	
	ldstr ""
	stloc.1
	
	ldloc.1
	stloc.0
	
	ret
}
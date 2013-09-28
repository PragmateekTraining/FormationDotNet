.assembly Safe{}

.method static void Main()
{
	.entrypoint

	.locals init (string, object)
	
	ldstr ""
	stloc.1
	
	ldloc.1
	castclass string
	stloc.0
	
	ret
}
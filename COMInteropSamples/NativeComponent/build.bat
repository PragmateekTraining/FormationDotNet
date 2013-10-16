CALL "C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\Tools\vsvars32.bat"
ECHO "Generating DLL..."
CL /LD /FeComLogger.dll ComLibrary.cpp ComLogger.cpp ComLoggerFactory.cpp ComLogger.def ole32.lib
ECHO "DLL generated."
ECHO "Generating TLB..."
MIDL ComLogger.idl
ECHO "TLB generated."
ECHO "Generating COM type library..."
TLBIMP ComLogger.tlb /out:ManagedCOMLogger.dll
ECHO "COM type library generated."
PAUSE
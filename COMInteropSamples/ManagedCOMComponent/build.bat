CALL "C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\Tools\vsvars32.bat"
ECHO "Generating TLB..."
TLBEXP COMInteropSamples.exe /out:ManagedCOMComponent\ManagedCOMLogger.tlb
ECHO "TLB generated."
ECHO "Registering assembly"
REGASM /codebase COMInteropSamples.exe
ECHO "Assembly registered"
CD ManagedCOMComponent
ECHO "Generating C++ test program..."
CL Test.cpp ole32.lib oleaut32.lib
ECHO "C++ test program generated."
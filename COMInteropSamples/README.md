Important
=========
As Visual Studio does not support COM registration of EXE projects, you must register the managed COM components manually.
It's what the build.bat script does by calling:
`regasm /codebase COMInteropSamples.exe`

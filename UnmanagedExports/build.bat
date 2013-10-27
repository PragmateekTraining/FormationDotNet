CALL "C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\Tools\vsvars32.bat"
LIB /DEF:UnmanagedExports.def
CL test.cpp UnmanagedExports.lib
PAUSE
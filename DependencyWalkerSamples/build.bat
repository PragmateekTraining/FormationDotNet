CALL "C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\Tools\vsvars32.bat"
CL /LD console.cpp
CL /LD logging.cpp console.lib
CSC /target:library /platform:x86 Logger.cs
PAUSE
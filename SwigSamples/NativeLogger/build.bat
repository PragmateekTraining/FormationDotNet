CALL "C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\Tools\vsvars32.bat"

ECHO "Compiling native logger..."
CL /LD NativeLogger.cpp
ECHO "Native logger compiled."

PAUSE
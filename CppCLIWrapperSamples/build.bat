CALL "C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\Tools\vsvars32.bat"

CD Native
ECHO "Compiling native logger..."
CL /LD NativeLogger.cpp
ECHO "Native logger compiled."

CD ..
COPY /Y Native\NativeLogger.lib Wrapper
COPY /Y Native\NativeLogger.dll Wrapper
COPY /Y Native\NativeLogger.h Wrapper

CD Wrapper
ECHO "Compiling C++/CLI wrapper..."
CL /clr /LD Logger.cpp NativeLogger.lib
ECHO "C++/CLI wrapper compiled."

CD ..
COPY /Y Wrapper\Logger.dll CSharp
COPY /Y Wrapper\NativeLogger.dll CSharp

CD CSharp
ECHO "Compiling C# application..."
CSC /r:Logger.dll /platform:x86 Test.cs
ECHO "C# application compiled."
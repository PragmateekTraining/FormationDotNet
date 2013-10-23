CALL "C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\Tools\vsvars32.bat"

..\..\Tools\SWIG\swig.exe -csharp -c++ -I../NativeLogger NativeLogger.i

COPY ..\NativeLogger\NativeLogger.lib .
COPY ..\NativeLogger\NativeLogger.dll .

CL /LD /I..\NativeLogger NativeLogger_wrap.cxx /FoNative NativeLogger.lib
COPY Native.dll ..
COPY NativeLogger.dll ..

PAUSE
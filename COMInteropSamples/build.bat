CALL "C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\Tools\vsvars32.bat"
COPY NativeComponent\ComLogger.lib ..
ECHO "Generating EXE..."
CL Test.cpp ComLogger.lib oleaut32.lib ole32.lib
ECHO "EXE generated."
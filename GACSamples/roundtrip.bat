CALL "C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\Tools\vsvars32.bat"

ECHO Installing assembly to GAC
gacutil /i GacUtilSampleAssembly.dll
PAUSE

ECHO Listing assemblies in GAC
gacutil /l GacUtilSampleAssembly
gacutil /l GacUtilSampleDependency
PAUSE

ECHO Installing assembly to GAC
gacutil /i GacUtilSampleDependency.dll
PAUSE

ECHO Listing assemblies in GAC
gacutil /l GacUtilSampleAssembly
gacutil /l GacUtilSampleDependency
PAUSE

ECHO Uninstalling assembly from GAC
gacutil /u GacUtilSampleDependency
gacutil /u GacUtilSampleAssembly
PAUSE
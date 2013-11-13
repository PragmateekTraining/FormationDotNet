CALL "C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\Tools\vsvars32.bat"
CSC /T:library /KEYFILE:GacUtilSampleKeys.snk GacUtilSampleDependency.cs
CSC /T:library /KEYFILE:GacUtilSampleKeys.snk /R:GacUtilSampleDependency.dll GacUtilSampleAssembly.cs
PAUSE
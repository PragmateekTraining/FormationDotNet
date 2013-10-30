CALL "C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\Tools\vsvars32.bat"
COPY ..\ShellAPI\Shell.API.dll .
CSC /target:library /reference:Shell.API.dll /reference:System.ComponentModel.Composition.dll LS.cs
CSC /target:library /reference:Shell.API.dll /reference:System.ComponentModel.Composition.dll Echo.cs
PAUSE
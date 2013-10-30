CALL "C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\Tools\vsvars32.bat"
CSC /target:library /out:Shell.API.dll ICommand.cs IShell.cs
PAUSE
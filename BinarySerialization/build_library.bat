CALL "C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\Tools\vsvars32.bat"

csc /target:library B.cs
csc /reference:B.dll /target:library A.cs
csc /target:library /out:C.dll B.cs

PAUSE
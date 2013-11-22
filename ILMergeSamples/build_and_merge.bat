CALL "C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\Tools\vsvars32.bat"

csc /target:library A.cs
csc /target:library B.cs
csc /target:library C.cs
ilmerge A.dll B.dll C.dll /out:ABC.dll

PAUSE
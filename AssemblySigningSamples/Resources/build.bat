CALL "C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\Tools\vsvars32.bat"
CSC /keyfile:..\company_key.snk /target:library /out:Resources.dll /linkresource:LinkedRootSite.config /resource:EmbeddedRootSite.config Resources.cs
PAUSE
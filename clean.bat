@ECHO OFF
%SYSTEMROOT%\Microsoft.NET\Framework\v3.5\msbuild.exe iSynaptic.Commons.msbuild /t:Clean /p:Configuration=Debug %*
%SYSTEMROOT%\Microsoft.NET\Framework\v3.5\msbuild.exe iSynaptic.Commons.msbuild /t:Clean /p:Configuration=Release %*

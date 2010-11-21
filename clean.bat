@ECHO OFF
%SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe iSynaptic.Commons.msbuild /t:Clean /p:Configuration=Debug %*
%SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe iSynaptic.Commons.msbuild /t:Clean /p:Configuration=Release %*

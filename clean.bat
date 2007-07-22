@ECHO OFF
%SYSTEMROOT%\Microsoft.NET\Framework\v2.0.50727\msbuild.exe master.build /t:MasterClean /p:Configuration=Debug %*
%SYSTEMROOT%\Microsoft.NET\Framework\v2.0.50727\msbuild.exe master.build /t:MasterClean /p:Configuration=Release %*
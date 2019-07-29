ECHO OFF
ECHO Starting PreBuild.bat %1 %2 %3
REM Usage: Call "$(MSBuildProjectDirectory)\PreBuild.$(Configuration).bat" "$(MSBuildProjectDirectory)" "$(Configuration)" "$(ProjectName)"
REM Vars:  $(ProjectName) = MyCo.Framework. Models, $(TargetPath) = output file, $(TargetDir) = full bin path , $(OutDir) = bin\debug, $(Configuration) = "Debug"

REM Locals
SET FullPath=%1
SET FullPath=%FullPath:"=%
ECHO FullPath: %FullPath%
SET Configuration=%2
ECHO Configuration: %Configuration%
SET ProjectName=%3
ECHO ProjectName: %ProjectName%

REM \App_Data\*.mdf
%WINDIR%\system32\attrib.exe "%FullPath%\*.mdf" -r /s
%WINDIR%\system32\attrib.exe "%FullPath%\*.ldf" -r /s

REM \App_Data\AppSettings.config
%WINDIR%\system32\attrib.exe "%FullPath%\App_Data\AppSettings.config" -r
%WINDIR%\system32\xcopy.exe  "%FullPath%\App_Data\AppSettings.%Configuration%.config" "%FullPath%\App_Data\AppSettings.config" /f/r/c/y

REM \App_Data\ConnectionStrings.config
%WINDIR%\system32\attrib.exe "%FullPath%\App_Data\ConnectionStrings.config" -r
%WINDIR%\system32\xcopy.exe  "%FullPath%\App_Data\ConnectionStrings.%Configuration%.Config" "%FullPath%\App_Data\ConnectionStrings.config" /f/r/c/y

ECHO Executing "%FullPath%\EFPartial.ps1" -Parameter1 "%FullPath%"

%WINDIR%\system32\attrib.exe "%FullPath%\*.cs" -r
%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\Powershell.exe Set-ExecutionPolicy Bypass -Scope Process -Force
%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\Powershell.exe -File "%FullPath%\EFPartial.ps1" -Parameter1 "%FullPath%"
%WINDIR%\system32\attrib.exe "%FullPath%\*.cs" +r

exit 0

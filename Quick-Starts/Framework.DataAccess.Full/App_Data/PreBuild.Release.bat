ECHO Starting PreBuild.bat %1 %2 %3
REM Usage: Call "$(MSBuildProjectDirectory)\App_Data\PreBuild.$(ConfigurationName).bat" "$(MSBuildProjectDirectory)" "$(ConfigurationName)" "$(ProjectName)"
REM Vars:  $(ProjectName) = MyCo.Framework. Models, $(TargetPath) = output file, $(TargetDir) = full bin path , $(OutDir) = bin\debug, $(ConfigurationName) = "Debug"

REM Locals
SET FullPath=%1
SET FullPath=%FullPath:"=%
ECHO FullPath: %FullPath%
SET Configuration=%2
ECHO Configuration: %Configuration%
SET ProjectName=%3
ECHO ProjectName: %ProjectName%

ECHO Executing "%FullPath%\EFPartial.ps1" -Parameter1 "%FullPath%"

%WINDIR%\system32\attrib.exe "%FullPath%\*.cs" -r
%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\Powershell.exe Set-ExecutionPolicy Bypass -Scope Process -Force
%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\Powershell.exe -File "%FullPath%\EFPartial.ps1" -Parameter1 "%FullPath%"
%WINDIR%\system32\attrib.exe "%FullPath%\*.cs" +r

exit 0
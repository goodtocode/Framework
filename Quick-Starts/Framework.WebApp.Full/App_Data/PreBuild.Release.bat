ECHO OFF
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

REM \App_Data\*.mdf
%WINDIR%\system32\attrib.exe "%FullPath%\*.mdf" -r /s
%WINDIR%\system32\attrib.exe "%FullPath%\*.ldf" -r /s

REM \App_Data\AppSettings.config
%WINDIR%\system32\attrib.exe "%FullPath%\App_Data\AppSettings.config" -r
%WINDIR%\system32\xcopy.exe  "%FullPath%\App_Data\AppSettings.%Configuration%.config" "%FullPath%\App_Data\AppSettings.config" /f/r/c/y

REM \App_Data\ConnectionStrings.config
%WINDIR%\system32\attrib.exe "%FullPath%\App_Data\ConnectionStrings.config" -r
%WINDIR%\system32\xcopy.exe  "%FullPath%\App_Data\ConnectionStrings.%Configuration%.Config" "%FullPath%\App_Data\ConnectionStrings.config" /f/r/c/y

exit 0

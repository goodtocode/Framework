ECHO Starting PreBuild.bat
REM Usage: Call "$(MSBuildProjectDirectory)\PreBuild.$(ConfigurationName).bat" "$(MSBuildProjectDirectory)" "$(ConfigurationName)"
REM Vars:  $(TargetPath) = output file, $(TargetDir) = full bin path , $(OutDir) = bin\debug, $(ConfigurationName) = "Debug"

REM Locals
SET FullPath=%1
SET FullPath=%FullPath:"=%
SET ProductFolder="Build.Test.Framework"

ECHO ** PreBuild.bat **
ECHO FullPath: %FullPath%
SET Configuration=%2
ECHO Configuration: %Configuration%

if "%Configuration%"=="" SET Configuration="Release"

REM \App_Data\*.mdf
%WINDIR%\system32\attrib.exe "%FullPath%\*.mdf" -r /s
%WINDIR%\system32\attrib.exe "%FullPath%\*.ldf" -r /s

REM \App_Data\AppSettings.config
%WINDIR%\system32\attrib.exe "%FullPath%\App_Data\AppSettings.config" -r
%WINDIR%\system32\xcopy.exe  "%FullPath%\App_Data\AppSettings.%Configuration%.config" "%FullPath%\App_Data\AppSettings.config" /f/r/c/y
%WINDIR%\system32\attrib.exe "%FullPath%\App_Data\AppSettings.config" +r

REM \App_Data\ConnectionStrings.config
%WINDIR%\system32\attrib.exe "%FullPath%\App_Data\ConnectionStrings.config" -r
%WINDIR%\system32\xcopy.exe  "%FullPath%\App_Data\ConnectionStrings.%Configuration%.Config" "%FullPath%\App_Data\ConnectionStrings.config" /f/r/c/y
%WINDIR%\system32\attrib.exe "%FullPath%\App_Data\ConnectionStrings.config" +r

exit 0

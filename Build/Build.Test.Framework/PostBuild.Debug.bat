ECHO OFF
REM Usage: Call "$(ProjectDir)PostBuild.$(ConfigurationName).bat" "$(TargetDir)" "$(TargetName)" "$(ConfigurationName)"
REM Vars:  $(TargetPath) = output file, $(TargetDir) = full bin path , $(OutDir) = bin\debug, $(ConfigurationName) = "Debug"

ECHO Starting PostBuild.bat

REM Locals
SET LibFolder=\lib\Genesys-Extensions
SET FullPath=%1%2
SET FullPath=%FullPath:"=%
SET FullPath="%FullPath%.*"
SET Configuration=%3
if "%Configuration%"=="" SET Configuration="Debug"
ECHO Configuration: %Configuration%


REM Additionally output to \lib folder
MD %LibFolder%
%WINDIR%\system32\attrib.exe %LibFolder%\*.* -r /s
%WINDIR%\system32\xcopy.exe %FullPath% %LibFolder%\*.* /d/f/s/e/r/c/y

REM Enable if want to copy all related Genesys dependencies to \lib (even those not of this Solution) 
REM %WINDIR%\system32\xcopy.exe "%PartialPath%Genesys.*" "%LibFolder%\*.*" /d/f/s/e/r/c/y

Exit 0

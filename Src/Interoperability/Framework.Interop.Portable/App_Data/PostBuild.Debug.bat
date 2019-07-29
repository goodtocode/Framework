ECHO OFF
ECHO Starting PostBuild.bat %1 %2 %3
REM Usage: Call "$(MSBuildProjectDirectory)\PostBuild.$(Configuration).bat" "$(MSBuildProjectDirectory)\$(OutDir)" "$(Configuration)" "$(ProjectName)"
REM Vars:  $(ProjectName) = MyCo.Framework. Models, $(TargetPath) = output file, $(TargetDir) = full bin path , $(OutDir) = bin\debug, $(Configuration) = "Debug"

REM Locals
SET FullPath=%1
SET FullPath=%FullPath:"=%
ECHO FullPath: %FullPath%
SET Configuration=%2
ECHO Configuration: %Configuration%
SET ProjectName=%3
ECHO ProjectName: %ProjectName%
SET LibFolder="\lib\GoodToCode-Framework"
ECHO LibFolder: %LibFolder%

REM Publish Output
MD %LibFolder%
%WINDIR%\system32\attrib.exe %LibFolder%\*.* -r /s
%WINDIR%\system32\xcopy.exe "%FullPath%\*.*" "%LibFolder%\*.*" /d/f/s/e/r/c/y
DEL "%LibFolder%\*.tmp" /f/q

Exit 0

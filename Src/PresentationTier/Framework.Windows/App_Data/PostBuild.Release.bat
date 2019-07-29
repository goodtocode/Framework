ECHO OFF
ECHO Starting PostBuild.bat%1 %2 %3
REM Usage: Call "$(MSBuildProjectDirectory)\PostBuild.$(Configuration).bat" "$(MSBuildProjectDirectory)\$(OutDir)" "$(Configuration)" "$(ProjectName)" "$(RootNameSpace)"
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
SET RootNamespace=%5
SET RootNamespace=%RootNamespace:"=%
ECHO RootNamespace: %RootNamespace%

REM Copying project output to build location
MD %LibFolder%
%WINDIR%\system32\attrib.exe %LibFolder%\*.* -r /s
%WINDIR%\system32\xcopy.exe %FullPath%\.* %LibFolder%\*.* /d/f/s/e/r/c/y
%WINDIR%\system32\xcopy.exe %1Properties\*.rd.xml %LibFolder%\%RootNamespace%\Properties\*.* /s/r/y
%WINDIR%\system32\xcopy.exe %FullPath%\*.png %LibFolder%\%RootNamespace%\*.* /s/r/y
%WINDIR%\system32\xcopy.exe %FullPath%\*.xbf %LibFolder%\%RootNamespace%\*.* /s/r/y
%WINDIR%\system32\xcopy.exe %FullPath%\*.xml %LibFolder%\%RootNamespace%\*.* /s/r/y
%WINDIR%\system32\xcopy.exe %FullPath%\*.* %LibFolder%\*.* /s/r/y
DEL "%LibFolder%\*.tmp" /f/q

Exit 0
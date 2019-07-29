ECHO OFF
ECHO Starting PostBuild.bat %1 %2 %3
REM Usage: Call "$(MSBuildProjectDirectory)\App_Data\PostBuild.$(Configuration).bat" "$(MSBuildProjectDirectory)\$(OutDir)" "$(Configuration)" "$(ProjectName)"
REM Vars:  $(ProjectName) = MyCo.Framework. Models, $(TargetPath) = output file, $(TargetDir) = full bin path , $(OutDir) = bin\debug, $(Configuration) = "Debug"

REM Locals
SET FullPath=%1
SET FullPath=%FullPath:"=%
ECHO FullPath: %FullPath%
SET Configuration=%2
ECHO Configuration: %Configuration%
SET ProjectName=%3
ECHO ProjectName: %ProjectName%
SET LibFolder="\lib\Genesys-Framework"
ECHO LibFolder: %LibFolder%

REM Copying project output to build location
MD %LibFolder%
%WINDIR%\system32\attrib.exe %LibFolder%\*.* -r /s
REM Properties\*.rd.xml -> Properties -> Copy to Output Directory: Copy always
%WINDIR%\system32\xcopy.exe %FullPath%\Properties\*.rd.xml %LibFolder%\%ProjectName%\Properties\*.* /s/r/y
REM Main Output
%WINDIR%\system32\xcopy.exe %FullPath%\*.dll %LibFolder%\*.* /f/r/c/y
%WINDIR%\system32\xcopy.exe %FullPath%\*.pdb %LibFolder%\*.* /f/r/c/y
%WINDIR%\system32\xcopy.exe %FullPath%\*.xml %LibFolder%\*.* /f/r/c/y
REM Markup and Content
%WINDIR%\system32\xcopy.exe %FullPath%\*.png %LibFolder%\%ProjectName%\*.* /f/s/e/r/c/y
%WINDIR%\system32\xcopy.exe %FullPath%\*.xbf %LibFolder%\%ProjectName%\*.* /f/s/e/r/c/y
%WINDIR%\system32\xcopy.exe %FullPath%\*.xml %LibFolder%\%ProjectName%\*.* /f/s/e/r/c/y
DEL "%LibFolder%\*.tmp" /f/q

Exit 0

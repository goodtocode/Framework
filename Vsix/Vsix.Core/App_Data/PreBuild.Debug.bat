ECHO OFF
ECHO Starting PreBuild.bat %1 %2 %3
REM Usage: Call "$(MSBuildProjectDirectory)\App_Data\PreBuild.$(Configuration).bat" "$(MSBuildProjectDirectory)" "$(Configuration)" "$(ProjectName)"
REM Vars:  $(ProjectName) = MyCo.Framework. Models, $(TargetPath) = output file, $(TargetDir) = full bin path , $(OutDir) = bin\debug, $(Configuration) = "Debug"

REM Locals
SET FullPath=%1
SET FullPath=%FullPath:"=%
ECHO FullPath: %FullPath%
SET Configuration=%2
ECHO Configuration: %Configuration%
SET ProjectName=%3
ECHO ProjectName: %ProjectName%
SET Source="..\..\Microservice.Vstemplate\Microservice.Template"
ECHO Source: %Source%
SET TemplateFolder="..\ProjectTemplates"
ECHO TemplateFolder: %TemplateFolder%

REM Copy template changes to VSIX project
REM MD %TemplateFolder%
REM %WINDIR%\system32\attrib.exe %TemplateFolder%\*.* -r /s
REM %WINDIR%\system32\xcopy.exe "%Source%\*.*" "%TemplateFolder%\*.*" /d/f/s/e/r/c/y

%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\Powershell.exe Set-ExecutionPolicy Bypass -Scope Process -Force
%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\Powershell.exe -File %FullPath%\App_Data\Compress-Template.ps1 -Path %Source% -ZipName Core.zip -Destination %TemplateFolder%
exit 0
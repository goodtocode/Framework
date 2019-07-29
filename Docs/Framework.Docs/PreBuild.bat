ECHO Starting PreBuild.bat
REM PreBuildEvent: Call "$(ProjectDir)PreBuild.Bat" "$(ProjectDir)"
REM Call "$(MSBuildProjectDirectory)\PreBuild.bat" "$(MSBuildProjectDirectory)"

REM Locals
SET FullPath=%1
SET FullPath=%FullPath:"=%
SET ProductFolder="framework.docs"

ECHO ** PreBuild.bat **
ECHO FullPath: %FullPath%
ECHO ProductFolder: %ProductFolder%

REM Rebuild Extensions and Framework
%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\Powershell.exe Set-ExecutionPolicy Bypass -Scope Process -Force
%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\Powershell.exe -File "..\..\Build\Build.Scripts.Modules\Callers\Restore-Solution.ps1" -Path "..\..\..\Extensions\Extensions.sln" -Configuration "Debug"
%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\Powershell.exe -File "..\..\Build\Build.Scripts.Modules\Callers\Restore-Solution.ps1" -Path  "..\..\..\Framework\Framework.sln" -Configuration "Debug"

exit 0
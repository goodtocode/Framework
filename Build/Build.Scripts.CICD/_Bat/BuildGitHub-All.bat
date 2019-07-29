ECHO OFF
REM -----------------------------------------------------------------------
REM <copyright file="BuildRepos.bat" company="Genesys Source">
REM      Copyright (c) Genesys Source. All rights reserved.
REM      All rights are reserved. Reproduction or transmission in whole or in part, in
REM      any form or by any means, electronic, mechanical or otherwise, is prohibited
REM      without the prior written consent of the copyright owner.
REM </copyright>
REM -----------------------------------------------------------------------

REM ***
REM Initialize
REM ***
ECHO Starting BuildCode.bat
CD \Source\Framework\3.00-Alpha\Build\Build.Scripts.CICD\_Bat
%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\Powershell.exe Set-ExecutionPolicy Bypass -Scope Process -Force

REM ***
REM Execute
REM ***
%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\Powershell.exe -File ..\GitHub\Extensions-GitHub.ps1 -Path \\Dev-Vm-01.dev.genesyssource.com\Vault\drops -Database DatabaseServer.dev.genesyssource.com -Domain code.genesyssource.com
%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\Powershell.exe -File ..\GitHub\Framework-GitHub.ps1 -Path \\Dev-Vm-01.dev.genesyssource.com\Vault\drops -Database DatabaseServer.dev.genesyssource.com -Domain code.genesyssource.com
%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\Powershell.exe -File ..\GitHub\Framework-GitHub-Mvc.ps1 -Path \\Dev-Vm-01.dev.genesyssource.com\Vault\drops -Database DatabaseServer.dev.genesyssource.com -Domain code.genesyssource.com
%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\Powershell.exe -File ..\GitHub\Framework-GitHub-WebApi.ps1 -Path \\Dev-Vm-01.dev.genesyssource.com\Vault\drops -Database DatabaseServer.dev.genesyssource.com -Domain code.genesyssource.com
%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\Powershell.exe -File ..\GitHub\Framework-GitHub-Wpf.ps1 -Path \\Dev-Vm-01.dev.genesyssource.com\Vault\drops -Database DatabaseServer.dev.genesyssource.com -Domain code.genesyssource.com
%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\Powershell.exe -File ..\GitHub\Framework-GitHub-Universal.ps1 -Path \\Dev-Vm-01.dev.genesyssource.com\Vault\drops -Database DatabaseServer.dev.genesyssource.com -Domain code.genesyssource.com

REM ***
REM Exit
REM ***
pause
exit 0
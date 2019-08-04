ECHO OFF
REM -----------------------------------------------------------------------
REM <copyright file="BuildGitHub-Universal.bat" company="GoodToCode">
REM      Copyright (c) GoodToCode. All rights reserved.
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
%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\Powershell.exe -File ..\GitHub\Framework-GitHub-Universal.ps1 -Path \\Dev-Vm-01.dev.GoodToCode.com\Vault\drops -Database DatabaseServer.dev.GoodToCode.com -Domain code.GoodToCode.com

REM ***
REM Exit
REM ***
pause
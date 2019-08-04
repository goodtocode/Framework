ECHO OFF
REM -----------------------------------------------------------------------
REM <copyright file="PackageGithub.bat" company="GoodToCode">
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
%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\Powershell.exe Set-ExecutionPolicy Bypass -Scope Process -Force

REM *** Set path for relatives to work
CD \Source\Framework\GitHub

REM ***
REM Execute
REM ***
REM Package all
ECHO Package GitHub...
%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\Powershell.exe -File .\Package-GitHub.ps1

REM ***
REM Exit
REM ***
pause
exit 0
ECHO OFF
REM -----------------------------------------------------------------------
REM <copyright file="ExecuteTests.bat" company="Genesys Source">
REM      Copyright (c) Genesys Source. All rights reserved.
REM      All rights are reserved. Reproduction or transmission in whole or in part, in
REM      any form or by any means, electronic, mechanical or otherwise, is prohibited
REM      without the prior written consent of the copyright owner.
REM </copyright>
REM -----------------------------------------------------------------------

REM ***
REM Initialize
REM ***
ECHO Starting ExecuteTests.bat
CD \Source\Content\3.00-Dev\Build\Build.Scripts.Modules\_Bat
%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\Powershell.exe Set-ExecutionPolicy Bypass -Scope Process -Force

REM Test Files
%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\Powershell.exe -File ..\_Tests\AdHocTests.ps1
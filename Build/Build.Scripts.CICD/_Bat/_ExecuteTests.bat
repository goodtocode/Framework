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
CD \Source\Framework\3.00-Alpha\Build\Build.Scripts.CICD\_Bat
%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\Powershell.exe Set-ExecutionPolicy Bypass -Scope Process -Force

REM Test
%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\Powershell.exe -File ..\Shopping-Cart\Framework-for-MVC-Shopping-Cart.ps1 -Path \\Dev-Web-01.dev.genesyssource.com -Database DatabaseServer.dev.genesyssource.com -Domain www.genesyssource.com
pause

REM Test
REM %WINDIR%\SysWOW64\WindowsPowerShell\v1.0\Powershell.exe -File ..\_Tests\Genesys.Code.Test.ps1


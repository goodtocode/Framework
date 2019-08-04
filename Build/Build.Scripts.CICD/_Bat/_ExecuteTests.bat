ECHO OFF
REM -----------------------------------------------------------------------
REM <copyright file="ExecuteTests.bat" company="GoodToCode">
REM      Copyright (c) GoodToCode. All rights reserved.
REM      All rights are reserved. Reproduction or transmission in whole or in part, in
REM      any form or by any means, electronic, mechanical or otherwise, is prohibited
REM      without the prior written consent of the copyright owner.
REM </copyright>
REM -----------------------------------------------------------------------
ECHO Starting ExecuteTests.bat
REM ***
REM Initialize
REM ***
REM CD to BAT file directory
D:
Set SourceDir = \Source-GTC\Stack\Framework
Set ArtifactDir = \Artifacts
Set PublisherToken = ub7dwhcn46oxeov7zyyhzfluomd3zdztqemhst4wbmk3lp4byega
Set PublisherName = GoodToCode

REM Set Script Security
%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\Powershell.exe Set-ExecutionPolicy Bypass -Scope Process -Force

REM ***
REM Test
REM ***
REM Publish-Vsix
%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\Powershell.exe -File %SourceDir%\Vsix\Publish-Vsix.ps1 -SourceDir %SourceDir% -ArtifactDir %ArtifactDir% -PublisherToken %PublisherToken% -PublisherName %PublisherName%
pause



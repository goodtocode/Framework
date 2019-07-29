ECHO OFF
REM -----------------------------------------------------------------------
REM <copyright file="BuildCodeVsix.bat" company="Genesys Source">
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
CD \Source\Framework\3.00-Dev\Build\Build.Test.Prod

REM ***
REM Execute
REM ***
ECHO WebTests - MVC
..\Utility\MsTest.exe /testcontainer:bin\debug\Genesys.Build.Test.Prod.dll
REM /category:"DeployTests&QuickStartTests"

REM ***
REM Exit
REM ***
Pause

ECHO OFF
REM -----------------------------------------------------------------------
REM <copyright file="NuGetCacheClear.bat" company="GoodToCode">
REM      Copyright (c) GoodToCode. All rights reserved.
REM      All rights are reserved. Reproduction or transmission in whole or in part, in
REM      any form or by any means, electronic, mechanical or otherwise, is prohibited
REM      without the prior written consent of the copyright owner.
REM </copyright>
REM -----------------------------------------------------------------------

REM ***
ECHO Initialize NuGetCacheClear.bat
REM ***

REM ***
ECHO Execute Extensions Repo to GitHub...
REM ***
nuget locals all -list
nuget locals all -clear

REM ***
ECHO Done
REM ***
Pause
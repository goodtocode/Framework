#-----------------------------------------------------------------------
# <copyright file="Set-Version.ps1" company="GoodToCode">
#      Copyright (c) GoodToCode. All rights reserved.
#      All rights are reserved. Reproduction or transmission in whole or in part, in
#      any form or by any means, electronic, mechanical or otherwise, is prohibited
#      without the prior written consent of the copyright owner.
# </copyright>
#-----------------------------------------------------------------------

# ***
# *** Parameters
# ***
param
(
    [string] $Path="C:\Temp"
)

# ***
# *** Initialize
# ***
Set-ExecutionPolicy Unrestricted -Scope Process -Force
$VerbosePreference = 'SilentlyContinue' # 'Continue'
[String]$ThisScript = $MyInvocation.MyCommand.Path
[String]$ThisDir = Split-Path $ThisScript
[DateTime]$Now = Get-Date
Set-Location $ThisDir # Ensure our location is correct, so we can use relative paths
Write-Host "*****************************"
Write-Host "*** Starting: $ThisScript on $Now"
Write-Host "*****************************"
# Imports
Import-Module "..\..\Build.Scripts.Modules\Code\GoodToCode.Code.psm1"
Import-Module "..\..\Build.Scripts.Modules\System\GoodToCode.System.psm1"

# ***
# *** Validate and cleanse
# ***
$Path = Set-Unc -Path $Path

# ***
# *** Locals
# ***
$PathFull = [String]::Format("{0}\Sites\{1}\{2}", $Path, $Domain, $SubFolder)

# ***
# *** Pre-Execute
# ***


# ***
# *** Execute
# ***
$GetVersion = Get-Version -Major 4
Write-Host "Version: $GetVersion"

Set-Version -Path $Path
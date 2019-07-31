#-----------------------------------------------------------------------
# <copyright file="GoodToCode.IO.Test.ps1" company="GoodToCode Source">
#      Copyright (c) GoodToCode Source. All rights reserved.
#      All rights are reserved. Reproduction or transmission in whole or in part, in
#      any form or by any means, electronic, mechanical or otherwise, is prohibited
#      without the prior written consent of the copyright owner.
# </copyright>
#-----------------------------------------------------------------------

# ***
# *** Parameters
# ***

# ***
# *** Initialize
# ***
Set-ExecutionPolicy Unrestricted -Scope Process -Force
$VerbosePreference = 'Continue'
[String]$ThisScript = $MyInvocation.MyCommand.Path
[String]$ThisDir = Split-Path $ThisScript
Set-Location $ThisDir # Ensure our location is correct, so we can use relative paths
Write-Host "*****************************"
Write-Host "*** Starting: $ThisScript on $(Get-Date -format 'u')"
Write-Host "*****************************"
# Imports
Import-Module "..\..\Build.Scripts.Modules\Code\GoodToCode.Code.psm1"

Import-Module "..\..\Build.Scripts.Modules\System\GoodToCode.System.psm1"


# Update line
Update-Text -Path 'c:\temp' -Include *.config -Old 'byehello' -New 'hello'

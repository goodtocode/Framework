#-----------------------------------------------------------------------
# <copyright file="Genesys.IO.Test.ps1" company="Genesys Source">
#      Copyright (c) Genesys Source. All rights reserved.
#      All rights are reserved. Reproduction or transmission in whole or in part, in
#      any form or by any means, electronic, mechanical or otherwise, is prohibited
#      without the prior written consent of the copyright owner.
# </copyright>
#-----------------------------------------------------------------------

# ***
# *** How-To
# ***
# Set-ExecutionPolicy RemoteSigned
# C:\Source\Framework\3.00-Alpha\Build\Build.Scripts.Modules\_Tests\AdHocTests.ps1


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
Import-Module "..\..\Build.Scripts.Modules\Code\Genesys.Code.psm1"
Import-Module "..\..\Build.Scripts.Modules\System\Genesys.System.psm1"

# TEST
Find-MsBuild
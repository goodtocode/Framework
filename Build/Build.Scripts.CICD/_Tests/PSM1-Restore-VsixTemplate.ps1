#-----------------------------------------------------------------------
# <copyright file="Adhoc-tests.ps1" company="GoodToCode">
#      Copyright (c) GoodToCode. All rights reserved.
#      All rights are reserved. Reproduction or transmission in whole or in part, in
#      any form or by any means, electronic, mechanical or otherwise, is prohibited
#      without the prior written consent of the copyright owner.
# </copyright>
#-----------------------------------------------------------------------

# ***
# *** Parameters
# ***
param(
	[String]$Path = '\\Dev-Web-01.dev.GoodToCode.com', 
	[String]$Build = '\\Dev-Vm-01.dev.GoodToCode.com\Vault\Builds\SprintsTest',
	[String]$Domain = 'code.GoodToCode.com',
	[String]$Database = 'DatabaseServer.dev.GoodToCode.com',
	[String]$SolutionFolder = 'Quick-Starts'
)

# ***
# *** Initialize
# ***
Set-ExecutionPolicy Unrestricted -Scope Process -Force
$VerbosePreference = 'SilentlyContinue' # 'Continue'
[String]$ThisScript = $MyInvocation.MyCommand.Path
[String]$ThisDir = Split-Path $ThisScript
Set-Location $ThisDir # Ensure our location is correct, so we can use relative paths
Write-Host "*****************************"
Write-Host "*** Starting: $ThisScript on $(Get-Date -format 'u')"
Write-Host "*****************************"

# Imports
Import-Module "..\..\Build.Scripts.Modules\Code\GoodToCode.Code.psm1"
Import-Module "..\..\Build.Scripts.Modules\System\GoodToCode.System.psm1"

# ***
# *** Execute
# ***
# Rebuild templates
Restore-VsixTemplate -Path "..\..\..\$SolutionFolder" -Destination $Path -Domain $Domain -Database $Database -FamilyName "Framework" -ProductFlavor "Universal" -Build $Build

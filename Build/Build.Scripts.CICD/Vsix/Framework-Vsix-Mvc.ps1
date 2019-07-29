#-----------------------------------------------------------------------
# <copyright file="Framework-Vsix-Quick-Start.ps1" company="Genesys Source">
#      Copyright (c) Genesys Source. All rights reserved.
#      All rights are reserved. Reproduction or transmission in whole or in part, in
#      any form or by any means, electronic, mechanical or otherwise, is prohibited
#      without the prior written consent of the copyright owner.
# </copyright>
#-----------------------------------------------------------------------

# ***
# *** Parameters
# ***
param(
	[String]$Path = '\\Dev-Vm-01.dev.genesyssource.com\Vault\Drops',
	[String]$Build = '\\Dev-Vm-01.dev.genesyssource.com\Vault\Builds\Sprints',
	[String]$Database = 'DatabaseServer.dev.genesyssource.com',	
	[String]$ProductName = 'Vsix-for-MVC',
	[String]$RepoName = 'Genesys-Framework',
	[String]$SubFolder = 'Vsix',
	[String]$Relative='..\..\',
	[String]$SolutionFolder = 'Quick-Start',
	[String]$Verbose = 'SilentlyContinue' # 'Continue'
)

# ***
# *** Initialize
# ***
Set-ExecutionPolicy Unrestricted -Scope Process -Force
$VerbosePreference = $Verbose
[String]$ThisScript = $MyInvocation.MyCommand.Path
[String]$ThisDir = Split-Path $ThisScript
Set-Location $ThisDir # Ensure our location is correct, so we can use relative paths
Write-Host "*****************************"
Write-Host "*** Starting: $ThisScript on $(Get-Date -format 'u')"
Write-Host "*****************************"

# Imports
Import-Module ($Relative + "Build.Scripts.Modules\Code\Genesys.Code.psm1")
Import-Module ($Relative + "Build.Scripts.Modules\System\Genesys.System.psm1")

# ***
# *** Validate and cleanse
# *** 
$Path = Set-Unc -Path $Path
$Build = Set-Unc -Path $Build

# ***
# *** Locals
# ***
$PathFull = [String]::Format("{0}\{1}\{2}", $Path, $SubFolder, $ProductName)
$BuildFull = [String]::Format("{0}\{1}\{2}\{3}", $Build, (Get-Date).ToString("yyyy.MM"), $SubFolder, $ProductName)

# ***
# *** Execute
# ***
# Rebuild templates
Restore-VsixTemplate -Path "..\..\..\$SolutionFolder" -Destination $PathFull -Database $Database -FamilyName "Framework" -ProductFlavor "Mvc" -Build $BuildFull

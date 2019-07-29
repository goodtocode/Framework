﻿#-----------------------------------------------------------------------
# <copyright file="Extensions-GitHub.ps1" company="Genesys Source">
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
	[String]$Database = 'DatabaseServer.dev.genesyssource.com',
	[String]$Build = '\\Dev-Vm-01.dev.genesyssource.com\Vault\builds\sprints',
	[String]$RepoName = 'Genesys-Extensions',
	[String]$ProductName = 'Extensions',
	[String]$SubFolder = 'GitHub',
	[String]$Lib='\lib',
	[String]$Relative='..\..\',
	[String]$SolutionFolder = ''
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
$GitHub = [string]::Format("{0}..\Build\Build.Content\GitHub\{1}", $Relative, $ProductName)

# ***
Write-Verbose "Builds"
# ***
New-Path -Path $BuildFull -Clean $True
Copy-SourceCode -Path $BuildFull -RepoName $RepoName -ProductName $ProductName -Snk GenesysFramework.snk
Copy-Recurse -Path $GitHub -Destination $BuildFull

# ***
Write-Verbose "Publish"
# ***
New-Path -Path ($PathFull + "\src") -Clean $True
New-Path -Path ($PathFull + "\lib") -Clean $True
Copy-Recurse -Path $BuildFull -Destination $PathFull
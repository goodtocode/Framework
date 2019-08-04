#-----------------------------------------------------------------------
# <copyright file="Build-Vsix-Core.ps1" company="GoodToCode">
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
	[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 	[string]$Path = $(throw '-Path is a required parameter. $(build.stagingDirectory)'),
	[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 	[string]$Build = $(throw '-Build is a required parameter. $(Build.SourcesDirectory)'),
	[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 	[string]$Database = $(throw '-Database is a required parameter. $(config.databaseServer)'),
	[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 	[String]$ProductFlavor = $(throw '-ProductFlavor is a required parameter.'),
	[String]$RepoName = 'GoodToCode-Framework',
	[String]$SubFolder = 'Vsix',
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
Import-Module ($Build + "\Build.Scripts.Modules\Code\GoodToCode.Code.psm1")
Import-Module ($Build + "\Build.Scripts.Modules\System\GoodToCode.System.psm1")

# ***
# *** Validate and cleanse
# *** 
$Path = Set-Unc -Path $Path
$Build = Set-Unc -Path $Build

# ***
# *** Locals
# ***
$ProductName = 'Vsix-for-' + $ProductFlavor
$PathFull = [String]::Format("{0}\{1}\{2}", $Path, $SubFolder, $ProductName)
$BuildFull = [String]::Format("{0}\{1}\{2}\{3}", $Build, (Get-Date).ToString("yyyy.MM"), $SubFolder, $ProductName)

# ***
# *** Execute
# ***
# Rebuild templates
Restore-VsixTemplate -Path "..\..\..\$SolutionFolder" -Destination $PathFull -Database $Database -FamilyName "Framework" -ProductFlavor $ProductFlavor -Build $BuildFull

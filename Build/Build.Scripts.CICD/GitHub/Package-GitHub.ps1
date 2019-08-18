#-----------------------------------------------------------------------
# <copyright file="Package-GitHub.ps1" company="GoodToCode">
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
	[String]$SourceDir = $(throw '-SourceDir is a required parameter.'),
	[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
	[String]$ArtifactDir = $(throw '-ArtifactDir is a required parameter. c:\github\repos\framework'),
	[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
	[String]$ProductName = $(throw '-ProductName is a required parameter. Framework')
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
Import-Module "$SourceDir\Build\Build.Scripts.Modules\Code\GoodToCode.Code.psm1"
Import-Module "$SourceDir\Build\Build.Scripts.Modules\System\GoodToCode.System.psm1"

# ***
# *** Validate and cleanse
# ***
$SourceDir = Set-Unc -Path $SourceDir
$ArtifactDir = Set-Unc -Path $ArtifactDir

# ***
# *** Locals
# ***
$GitHubContent = "$SourceDir\Build\Build.Content\GitHub\$ProductName"

# ***
# *** Execute
# ***
New-Path -Path $ArtifactDir -Clean $True
Copy-Recurse -Path $SourceDir -Destination $ArtifactDir
Copy-Recurse -Path $GitHubContent -Destination $ArtifactDir
Clear-Solution -Path "$ArtifactDir\Src"
Clear-Solution -Path "$ArtifactDir\Quick-Starts"
Remove-Path -Path "$ArtifactDir\.vs"
Remove-Path -Path "$ArtifactDir\Build"
Remove-Path -Path "$ArtifactDir\Docs"
Remove-Path -Path "$ArtifactDir\Vsix"
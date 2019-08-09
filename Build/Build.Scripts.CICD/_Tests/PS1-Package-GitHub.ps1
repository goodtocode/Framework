#-----------------------------------------------------------------------
# <copyright file="Framework-NuGet.ps1" company="GoodToCode">
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
	[String]$SourceDir = 'C:\Users\rober\source\repos\Framework', # D:\Source-GTC\Stack\Framework',
	[String]$ArtifactDir = 'C:\Artifacts\t', 	
	[String]$ProductName = 'Framework'
)

# ***
# *** Initialize
# ***
Set-ExecutionPolicy Unrestricted -Scope Process -Force
$VerbosePreference = 'Continue' # 'SilentlyContinue'
[String]$ThisScript = $MyInvocation.MyCommand.Path
[String]$ThisDir = Split-Path $ThisScript
[DateTime]$Now = Get-Date
Set-Location $ThisDir # Ensure our location is correct, so we can use relative paths
Write-Host "*****************************"
Write-Host "*** Starting: $ThisScript on $Now"
Write-Host "*****************************"
# Imports
Import-Module ($SourceDir + '\Build\Build.Scripts.Modules\Code\GoodToCode.Code.psm1')
Import-Module ($SourceDir + '\Build\Build.Scripts.Modules\System\GoodToCode.System.psm1')

# ***
# *** Validate and cleanse
# ***
$ArtifactDir = Set-Unc -Path $ArtifactDir
$SourceDir = Set-Unc -Path $SourceDir

# ***
# *** Locals
# ***


# ***
# *** Execute
# ***
# Publish-Vsix
& "$SourceDir\Build\Build.Scripts.CICD\GitHub\Package-GitHub.ps1" -SourceDir $SourceDir -ArtifactDir $ArtifactDir -ProductName $ProductName
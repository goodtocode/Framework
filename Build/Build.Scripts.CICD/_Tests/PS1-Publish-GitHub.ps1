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
	[String]$TempDir = 'C:\Artifacts\t',
	[String]$ArtifactDir = 'C:\Artifacts\a',
	[String]$OrgName = 'GoodToCode',
	[String]$RepoName = 'Framework'	
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

# ***
# *** Validate and cleanse
# ***

# ***
# *** Locals
# ***

# ***
# *** Execute
# ***
# Publish-Vsix
& "..\GitHub\Publish-GitHub.ps1" -ArtifactDir $ArtifactDir -TempDir $TempDir -OrgName $OrgName -RepoName $RepoName
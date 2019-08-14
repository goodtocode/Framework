#-----------------------------------------------------------------------
# <copyright file="Publish-GitHub.ps1" company="GoodToCode">
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
	[String]$ArtifactDir = $(throw '-ArtifactDir is a required parameter. c:\builds\1\a'),
	[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
	[String]$TempDir = $(throw '-TempDir is a required parameter. c:\builds\1\t'),
	[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
	[String]$Url = $(throw '-Url is a required parameter. https://github.com/goodtocode/Framework.git'),
	[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
	[String]$RepoName = $(throw '-RepoName is a required parameter. Framework'),
	[String]$User = 'Robert Good',
	[String]$Email = 'robert.good@goodtocode.com'
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

# ***
# *** Locals
# ***
[String]$YearMo = (get-date).year.ToString()+'.'+(get-date).month.ToString("00")
[String]$RepoDir = "$TempDir\$RepoName"

# ***
# *** Execute
# ***
If(-not (Test-Path -PathType Container -Path $ArtifactDir)) {New-Item -Path $ArtifactDir -ItemType directory -Force}
If(-not (Test-Path -PathType Container -Path $TempDir)) {New-Item -Path $TempDir -ItemType directory -Force}
Set-Location $TempDir
& git config --global user.name $User
& git config --global user.email $Email
& git clone $Url
If(-not (Test-Path -PathType Container -Path $RepoDir)) {New-Item -Path "$TempDir\$RepoName" -ItemType directory -Force}
Set-Location $RepoDir
Get-ChildItem -Path $ArtifactDir | % { 
  Copy-Item $_.fullname "$RepoDir" -Recurse -Force 
}
& git add .
& git commit -m "Iteration $YearMo"
#git push -u origin master



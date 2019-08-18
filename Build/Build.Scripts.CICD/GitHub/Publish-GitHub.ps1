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
	[String]$OrgName = $(throw '-OrgName is a required parameter. GoodToCode'),
	[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
	[String]$RepoName = $(throw '-RepoName is a required parameter. Framework'),
	[String]$User = 'GoodToCode',
	[String]$Email = 'robert.good@goodtocode.com',
	[String]$PlatformDomain = 'github.com'
)
function Get-BasicAuthCreds {
    param([string]$Username,[string]$Password)
    $AuthString = "{0}:{1}" -f $Username,$Password
    $AuthBytes  = [System.Text.Encoding]::Ascii.GetBytes($AuthString)
    return [Convert]::ToBase64String($AuthBytes)
}

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
[String]$PlatformUrl = "https://$PlatformDomain"
[String]$RepoUrl = "https://$PlatformDomain/$OrgName/$RepoName.git"
[String]$RepoUrlUser = "https://$PlatformDomain/$OrgName/$RepoName.git"
[String]$RepoSsh = 'git@' + $PlatformDomain + ':' + $OrgName + '/' + $RepoName + '.git'
# ***
# *** Execute
# ***
If(-not (Test-Path -PathType Container -Path $ArtifactDir)) {New-Item -Path $ArtifactDir -ItemType directory -Force}
If(-not (Test-Path -PathType Container -Path $TempDir)) {New-Item -Path $TempDir -ItemType directory -Force}
Set-Location $TempDir

#
# GitHub 
#
# RepoHttps 'https://github.com/goodtocode/Framework.git'
# RepoSsh 'git@github.com:goodtocode/Framework.git'
# Dynamic: $RepoUrl = "$PlatformUrl/$OrgName/$RepoName.git"
# Pat as user (pops up login) $RepoUrl = "https://$Pat@$PlatformDomain/$OrgName/$RepoName.git"
# Command fails: & cmdkey /generic:"git:$PlatformUrl" /user:"Personal Access Token" /pass:"$Pat"
# Didnt work: $BasicCreds = Get-BasicAuthCreds -Username $Email -Password $Pw; Invoke-WebRequest -Uri $RepoUrl -Headers @{"Authorization"="Basic $BasicCreds"};
#[String]$Pw = 'Virtualkidd1'; & cmdkey /generic:"git:$PlatformUrl" /user:"$User" /pass:"$Pw"

# Ssh Agent
# Start-Service ssh-agent
# ssh-add .ssh/github.framework
# ssh "$User" + "@" + $PlatformDomain

# config
& git config --global user.name $User
& git config --global user.email $Email
& git config remote.origin.url $RepoSsh
& git remote set-url origin $RepoSsh

# clone
& git clone "ssh://$RepoSsh"
If(-not (Test-Path -PathType Container -Path $RepoDir)) {New-Item -Path "$TempDir\$RepoName" -ItemType directory -Force}
Set-Location $RepoDir
# Init
& git init

# Update with latest files
Get-ChildItem -Path $ArtifactDir | % { 
  Copy-Item $_.fullname "$RepoDir" -Recurse -Force 
}

# Push
& git add .
& git commit -m "Iteration $YearMo"
& git push -u origin master
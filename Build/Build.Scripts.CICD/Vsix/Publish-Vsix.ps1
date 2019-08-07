#-----------------------------------------------------------------------
# <copyright file="Publish-Vsix.ps1" company="GoodToCode">
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
 	[string]$ArtifactDir = $(throw '-Path is a required parameter. $(Build.ArtifactStagingDirectory)'),
	[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 	[string]$SourceDir = $(throw '-Build is a required parameter. $(Build.SourcesDirectory)'),
	[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 	[string]$PublisherToken = $(throw '-PublisherToken is a required parameter.'),
	[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
	[Version]$PublisherName= $(throw '-PublisherName is a required parameter. GoodToCode'),
	[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
	[Version]$Version= $(throw '-Version is a required parameter. $(Build.BuildNumber)'),
	[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
	[Version]$ProductFlavor= $(throw '-$ProductFlavor is a required parameter. Core')
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
$ArtifactDir = Set-Unc -Path $ArtifactDir
$SourceDir = Set-Unc -Path $SourceDir

# ***
# *** Locals
# ***
# VSIX Files
[String]$ProjectFolder = Set-Unc "$SourceDir\Vsix\Vsix.$ProductFlavor"
[String]$VsixPublisherExe = (Set-Unc "$SourceDir\Build\Build.Content\Utility\BuildTools") + '\VsixPublisher.exe'

# ***
# *** Execute
# ***
# Update version
Update-TextByContains -Path $ArtifactDir -Contains "<Identity Id" -Old "4.19.01" -New $Version.ToString() -Include *.vsixmanifest
# Publish VSIX
& $VsixPublisherExe login -publisherName $PublisherName -personalAccessToken $PublisherToken;
& $VsixPublisherExe publish -payload "$ArtifactDir\$ProductFlavor.vsix" -publishManifest "$ProjectFolder\publishManifest.json" -ignoreWarnings "VSIXValidatorWarning01,VSIXValidatorWarning02,VSIXValidatorWarning08" -personalAccessToken $PublisherToken;

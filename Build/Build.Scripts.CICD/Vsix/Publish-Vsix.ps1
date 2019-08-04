#-----------------------------------------------------------------------
# <copyright file="Framework-Vsix-Quick-Start.ps1" company="GoodToCode">
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
 	[string]$Path = $(throw '-Path is a required parameter.'),
	[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 	[string]$Build = $(throw '-Build is a required parameter. $(Build.SourcesDirectory)'),
	[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 	[string]$PublisherToken = $(throw '-PublisherToken is a required parameter.'),
	[String]$PublisherName = 'GoodToCode',
	[String]$ProductFlavor = 'Core'
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
Import-Module ($Build + "\Build\Build.Scripts.Modules\Code\GoodToCode.Code.psm1")
Import-Module ($Build + "\Build\Build.Scripts.Modules\System\GoodToCode.System.psm1")

# ***
# *** Validate and cleanse
# *** 
$Path = Set-Unc -Path $Path
$Build = Set-Unc -Path $Build

# ***
# *** Locals
# ***
# VSIX Files
[String]$ProjectFolder = Set-Unc ($Build + '\Vsix\Vsix.' + $ProductFlavor)
[String]$VsixPublisherExe = (Set-Unc ($Build + '\Build\Build.Content\Utility\BuildTools')) + '\VsixPublisher.exe'
$PublishManifestFile = $ProjectFolder + '\publishManifest.json'
$VsixArtifact = "$Path\Vsix-for-$ProductFlavor\$ProductFlavor.vsix"

# ***
# *** Execute
# ***
# Publish VSIX
& $VsixPublisherExe login -personalAccessToken $PublishToken -publisherName $PublisherName -personalAccessToken $PublisherToken;
& $VsixPublisherExe publish -payload $VsixArtifact -publishManifest $PublishManifestFile -ignoreWarnings "VSIXValidatorWarning01,VSIXValidatorWarning02" -personalAccessToken $PublisherToken;

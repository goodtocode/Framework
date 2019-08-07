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
 	[string]$ArtifactDir = $(throw '-Path is a required parameter. $(build.stagingDirectory)'),
	[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 	[string]$SourceDir = $(throw '-Build is a required parameter. $(Build.SourcesDirectory)'),
	[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
	[Version] $Version= $(throw '-Version is a required parameter. $(Build.BuildNumber)')
	[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 	[String]$ProductFlavor = $(throw '-ProductFlavor is a required parameter.'),
	[String]$TempDir = "C:\Temp"
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
Import-Module ($SourceDir + "\Build\Build.Scripts.Modules\Code\GoodToCode.Code.psm1")
Import-Module ($SourceDir + "\Build\Build.Scripts.Modules\System\GoodToCode.System.psm1")

# ***
# *** Validate and cleanse
# *** 
$ArtifactDir = Set-Unc -Path $ArtifactDir
$SourceDir = Set-Unc -Path $SourceDir
$TempDir = Set-Unc -Path $TempDir

# ***
# *** Locals
# ***
[String]$ContentPath="$SourceDir\Build\Build.Content"
[String]$TempDirZipPath="$TempDir\ToZip"
[String]$TempDirZipFile="$TempDir\$ProductFlavor.zip"
# Vsix
[String]$VsixProjectTemplateZip="$SourceDir\Vsix\Vsix.$ProductFlavor\ProjectTemplates\$ProductFlavor.zip"

# ***
# *** Execute
# ***
#
# Build quick-start Zip
#
New-Path -Path $TempDirZipPath -Clean $True
Copy-Recurse -Path "$SourceDir\Quick-Starts" -Destination $TempDirZipPath -Exclude *.dbmdl, *.jfm
Clear-Solution -Path $TempDirZipPath
Remove-Recurse -Path $TempDirZipPath -Include *.sln, *.vstemplate
Copy-Recurse -Path ("$ContentPath\Vsix") -Destination $TempDirZipPath -Include *.vstemplate
Copy-File -Path ("$ContentPath\Vsix\__PreviewImage.png") -Destination $TempDirZipPath
Copy-File -Path ("$ContentPath\Vsix\__TemplateIcon$ProductFlavor.png") -Destination $TempDirZipPath
Rename-File -Path ("$TempDirZipPath\$ProductFlavor.vstemplate") -NewName root.bak
Remove-File -Path ("$TempDirZipPath\*.vstemplate")
Rename-File -Path ("$TempDirZipPath\root.bak") -NewName root.vstemplate
if ($ProductFlavor -eq 'Core') {
		Remove-Path -Path "$TempDirZipPath\Framework.Android"
		Remove-Path -Path "$TempDirZipPath\Framework.DataAccess.Full"
		Remove-Path -Path "$TempDirZipPath\Framework.DesktopApp"
		Remove-Path -Path "$TempDirZipPath\Framework.Interop.Portable"
		Remove-Path -Path "$TempDirZipPath\Framework.iOS"
		Remove-Path -Path "$TempDirZipPath\Framework.Models.Portable"
		Remove-Path -Path "$TempDirZipPath\Framework.Test.Full"
		Remove-Path -Path "$TempDirZipPath\Framework.UniversalApp.Full"
		Remove-Path -Path "$TempDirZipPath\Framework.WebApp.Full"
		Remove-Path -Path "$TempDirZipPath\Framework.WebServices.Full"
	}
if ($ProductFlavor -eq 'MVC') {
		Remove-Path -Path "$TempDirZipPath\Framework.Android"
		Remove-Path -Path "$TempDirZipPath\Framework.DesktopApp"
		Remove-Path -Path "$TempDirZipPath\Framework.iOS"
		Remove-Path -Path "$TempDirZipPath\Framework.UniversalApp.Full"
		Remove-Path -Path "$TempDirZipPath\Framework.UniversalApp.Core"
		Remove-Path -Path "$TempDirZipPath\Framework.WebServices.Core"
		Remove-Path -Path "$TempDirZipPath\Framework.WebServices.Full"
	}
	if ($ProductFlavor -eq 'UWP') {
		Remove-Path -Path "$TempDirZipPath\Framework.Android"
		Remove-Path -Path "$TempDirZipPath\Framework.DesktopApp"
		Remove-Path -Path "$TempDirZipPath\Framework.iOS"
		Remove-Path -Path "$TempDirZipPath\Framework.WebApp.Core"
		Remove-Path -Path "$TempDirZipPath\Framework.WebApp.Full"
}
if ($ProductFlavor -eq 'WebAPI') {
		Remove-Path -Path "$TempDirZipPath\Framework.Android"
		Remove-Path -Path "$TempDirZipPath\Framework.DesktopApp"
		Remove-Path -Path "$TempDirZipPath\Framework.iOS"
		Remove-Path -Path "$TempDirZipPath\Framework.UniversalApp.Full"
		Remove-Path -Path "$TempDirZipPath\Framework.UniversalApp.Core"
		Remove-Path -Path "$TempDirZipPath\Framework.WebApp.Core"
		Remove-Path -Path "$TempDirZipPath\Framework.WebApp.Full"
}
if ($ProductFlavor -eq 'WPF') {
		Remove-Path -Path "$TempDirZipPath\Framework.Android"
		Remove-Path -Path "$TempDirZipPath\Framework.iOS"
		Remove-Path -Path "$TempDirZipPath\Framework.UniversalApp.Full"
		Remove-Path -Path "$TempDirZipPath\Framework.UniversalApp.Core"
		Remove-Path -Path "$TempDirZipPath\Framework.WebApp.Core"
		Remove-Path -Path "$TempDirZipPath\Framework.WebApp.Full"
}
# Fix: Dependencies won't load unless we change ..\packages to ..\..\packages. NuGet and VSIX want solution folder in different levels, so we compensate for VSIX
Update-Text -Path $TempDir -Include *.csproj -Old '..\packages' -New '..\..\packages' 
# Update version
Update-TextByContains -Path $TempDir -Contains "<Identity Id" -Old "4.19.01" -New $Version -Include *.vsixmanifest

#
# Zip
#
Compress-Path -Path $TempDirZipPath -File $TempDirZipFile

#
# Build VSIX
#
Set-ItemProperty $VsixProjectTemplateZip -name IsReadOnly -value $false
Copy-File -Path $TempDirZipFile -Destination $VsixProjectTemplateZip
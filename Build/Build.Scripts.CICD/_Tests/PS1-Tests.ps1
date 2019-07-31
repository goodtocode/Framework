#-----------------------------------------------------------------------
# <copyright file="Framework-NuGet.ps1" company="GoodToCode Source">
#      Copyright (c) GoodToCode Source. All rights reserved.
#      All rights are reserved. Reproduction or transmission in whole or in part, in
#      any form or by any means, electronic, mechanical or otherwise, is prohibited
#      without the prior written consent of the copyright owner.
# </copyright>
#-----------------------------------------------------------------------

# ***
# *** Parameters
# ***
param(
	[String]$Path = '\\Dev-Vm-01.dev.GoodToCode.com\Vault\Drops', 
	[String]$Domain = 'nuget.GoodToCode.com',
	[String]$Database = 'DatabaseServer.dev.GoodToCode.com',
	[String]$ProductName = "Entities",
	[String]$RepoName = "GoodToCode-Entities",
	[String]$Build = '\\Dev-Vm-01.dev.GoodToCode.com\Vault\Builds\Sprints',
	[String]$SubFolder = 'Packages',
	[String]$Lib='\lib',
	[String]$Relative='..\..\'
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
Import-Module ($Relative + 'Build.Scripts.Modules\Code\GoodToCode.Code.psm1')
Import-Module ($Relative + 'Build.Scripts.Modules\System\GoodToCode.System.psm1')

# ***
# *** Validate and cleanse
# ***
$Path = Set-Unc -Path $Path
$Build = Set-Unc -Path $Build

# ***
# *** Locals
# ***
$PathFull = [String]::Format("{0}\Sites\{1}\{2}", $Path, $Domain, $SubFolder)
$BuildFull = [String]::Format("{0}\{1}\{2}\{3}", $Build, (Get-Date).ToString("yyyy.MM"), $Domain, $SubFolder)
[String]$Nuget= Convert-PathSafe -Path "$Relative..\Build\Build.Content\NuGet"
[String]$Lib=[String]::Format("\lib\{0}", $RepoName)
[String]$Solution = [String]::Format("{0}..\{1}\{1}.sln", $Relative, $ProductName)
[String]$NuGetSpecFile = ""
[String]$AssemblyPath = ""

# ***
# *** Execute
# ***
# Build 
#Restore-Solution -Path $Solution -Configuration Debug -DevEnv $True

# NuGet: Framework.DataAccess.Core
$AssemblyPath=[String]::Format("{0}\*.DataAccess.*", $Lib)
$NuGetSpecFile = [String]::Format("{0}\Entity.DataAccess.Core.nuspec", $Nuget)
Add-NuGet -Path $Lib -NuSpec $NuGetSpecFile
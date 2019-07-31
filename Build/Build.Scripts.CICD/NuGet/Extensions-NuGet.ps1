#-----------------------------------------------------------------------
# <copyright file="Extensions-NuGet.ps1" company="GoodToCode Source">
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
	[String]$ProductName = "Extensions",
	[String]$RepoName = "GoodToCode-Extensions",
	[String]$Build = '\\Dev-Vm-01.dev.GoodToCode.com\Vault\Builds\Sprints',
	[String]$SubFolder = 'Packages',
	[String]$Lib='\lib'
)

# ***
# *** Initialize
# ***
Set-ExecutionPolicy Unrestricted -Scope Process -Force
$VerbosePreference = 'SilentlyContinue' # 'Continue'
[String]$ThisScript = $MyInvocation.MyCommand.Path
[String]$ThisDir = Split-Path $ThisScript
[DateTime]$Now = Get-Date
Set-Location $ThisDir # Ensure our location is correct, so we can use relative paths
Write-Host "*****************************"
Write-Host "*** Starting: $ThisScript on $Now"
Write-Host "*****************************"
# Imports
Import-Module "..\..\Build.Scripts.Modules\Code\GoodToCode.Code.psm1"
Import-Module "..\..\Build.Scripts.Modules\System\GoodToCode.System.psm1"

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
[String]$Nuget= Convert-PathSafe -Path "..\..\..\Build\Build.Content\NuGet"
[String]$Lib=[String]::Format("\lib\{0}", $RepoName)
[String]$Solution = [String]::Format("..\..\..\{0}\{0}.sln", $ProductName)
[String]$NuGetSpecFile = ""
[String]$AssemblyPath = ""

# ***
# *** Pre-Execute
# ***
# None needed. Add script only, no overwrite, no delete.

# ***
# *** Execute
# ***
# Build 
Restore-Solution -Path $Solution -Configuration Debug

# ***
# NuGet: Extensions.Portable
# ***
$AssemblyPath=[String]::Format("{0}\*.Portable.*", $Lib)
$NuGetSpecFile = [String]::Format("{0}\{1}.Portable.nuspec", $Nuget, $ProductName)
Copy-Recurse -Path $AssemblyPath -Destination $BuildFull -Include *.nupkg -Overwrite $false
Copy-Recurse -Path $AssemblyPath -Destination $PathFull -Include *.nupkg -Overwrite $false
Add-NuGet -Path $Lib -NuSpec $NuGetSpecFile

# ***
# NuGet: Extensions.Full
# ***
$AssemblyPath=[String]::Format("{0}\*.Full.*", $Lib)
$NuGetSpecFile = [String]::Format("{0}\{1}.Full.nuspec", $Nuget, $ProductName)
Copy-Recurse -Path $AssemblyPath -Destination $BuildFull -Include *.nupkg -Overwrite $false
Copy-Recurse -Path $AssemblyPath -Destination $PathFull -Include *.nupkg -Overwrite $false
Add-NuGet -Path $Lib -NuSpec $NuGetSpecFile

# ***
# NuGet: Extensions.Standard
# ***
$AssemblyPath=[String]::Format("{0}\*.Standard.*", $Lib)
$NuGetSpecFile = [String]::Format("{0}\{1}.Standard.nuspec", $Nuget, $ProductName)
Copy-Recurse -Path $AssemblyPath -Destination $BuildFull -Include *.nupkg -Overwrite $false
Copy-Recurse -Path $AssemblyPath -Destination $PathFull -Include *.nupkg -Overwrite $false
Add-NuGet -Path $Lib -NuSpec $NuGetSpecFile

# ***
# NuGet: Extensions.Core
# ***
$AssemblyPath=[String]::Format("{0}\*.Core.*", $Lib)
$NuGetSpecFile = [String]::Format("{0}\{1}.Core.nuspec", $Nuget, $ProductName)
Copy-Recurse -Path $AssemblyPath -Destination $BuildFull -Include *.nupkg -Overwrite $false
Copy-Recurse -Path $AssemblyPath -Destination $PathFull -Include *.nupkg -Overwrite $false
Add-NuGet -Path $Lib -NuSpec $NuGetSpecFile
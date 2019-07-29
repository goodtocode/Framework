#-----------------------------------------------------------------------
# <copyright file="Framework-NuGet.ps1" company="Genesys Source">
#      Copyright (c) Genesys Source. All rights reserved.
#      All rights are reserved. Reproduction or transmission in whole or in part, in
#      any form or by any means, electronic, mechanical or otherwise, is prohibited
#      without the prior written consent of the copyright owner.
# </copyright>
#-----------------------------------------------------------------------

# ***
# *** Parameters
# ***
param(
	[String]$Path = '\\Dev-Vm-01.dev.genesyssource.com\Vault\Drops', 
	[String]$Domain = 'nuget.genesyssource.com',
	[String]$Database = 'DatabaseServer.dev.genesyssource.com',
	[String]$ProductName = "Framework",
	[String]$RepoName = "Genesys-Framework",
	[String]$Build = '\\Dev-Vm-01.dev.genesyssource.com\Vault\Builds\Sprints',
	[String]$SubFolder = 'Packages',
	[String]$Lib='\lib',
	[String]$Relative='..\..\'
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
Import-Module ($Relative + 'Build.Scripts.Modules\Code\Genesys.Code.psm1')
Import-Module ($Relative + 'Build.Scripts.Modules\System\Genesys.System.psm1')

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
Restore-Solution -Path $Solution -Configuration Debug -DevEnv $True
# Update version
Update-ContentsByTag -Path $Nuget -Value (Get-Version -Major 4) -Open '<version>' -Close '</version>' -Include *.nuspec

# NuGet: Framework.Interop.Standard
$AssemblyPath=[String]::Format("{0}\*.Interop.*", $Lib)
$NuGetSpecFile = [String]::Format("{0}\{1}.Interop.Standard.nuspec", $Nuget, $ProductName)
Copy-Recurse -Path $AssemblyPath -Destination $BuildFull -Include *.nupkg -Overwrite $false
Copy-Recurse -Path $AssemblyPath -Destination $PathFull -Include *.nupkg -Overwrite $false
Add-NuGet -Path $Lib -NuSpec $NuGetSpecFile

# NuGet: Framework.Interop.Portable
$AssemblyPath=[String]::Format("{0}\*.Interop.*", $Lib)
$NuGetSpecFile = [String]::Format("{0}\{1}.Interop.Portable.nuspec", $Nuget, $ProductName)
Copy-Recurse -Path $AssemblyPath -Destination $BuildFull -Include *.nupkg -Overwrite $false
Copy-Recurse -Path $AssemblyPath -Destination $PathFull -Include *.nupkg -Overwrite $false
Add-NuGet -Path $Lib -NuSpec $NuGetSpecFile

# NuGet: Framework.DataAccess.Full
$AssemblyPath=[String]::Format("{0}\*.DataAccess.*", $Lib)
$NuGetSpecFile = [String]::Format("{0}\{1}.DataAccess.Full.nuspec", $Nuget, $ProductName)
Copy-Recurse -Path $AssemblyPath -Destination $BuildFull -Include *.nupkg -Overwrite $false
Copy-Recurse -Path $AssemblyPath -Destination $PathFull -Include *.nupkg -Overwrite $false
Add-NuGet -Path $Lib -NuSpec $NuGetSpecFile

# NuGet: Framework.DataAccess.Core
$AssemblyPath=[String]::Format("{0}\*.DataAccess.*", $Lib)
$NuGetSpecFile = [String]::Format("{0}\{1}.DataAccess.Core.nuspec", $Nuget, $ProductName)
Copy-Recurse -Path $AssemblyPath -Destination $BuildFull -Include *.nupkg -Overwrite $false
Copy-Recurse -Path $AssemblyPath -Destination $PathFull -Include *.nupkg -Overwrite $false
Add-NuGet -Path $Lib -NuSpec $NuGetSpecFile

# NuGet: Framework.Models.Standard
$AssemblyPath=[String]::Format("{0}\*.Models.*", $Lib)
$NuGetSpecFile = [String]::Format("{0}\{1}.Models.Standard.nuspec", $Nuget, $ProductName)
Copy-Recurse -Path $AssemblyPath -Destination $BuildFull -Include *.nupkg -Overwrite $false
Copy-Recurse -Path $AssemblyPath -Destination $PathFull -Include *.nupkg -Overwrite $false
Add-NuGet -Path $Lib -NuSpec $NuGetSpecFile

# NuGet: Framework.Models.Portable
$AssemblyPath=[String]::Format("{0}\*.Models.*", $Lib)
$NuGetSpecFile = [String]::Format("{0}\{1}.Models.Portable.nuspec", $Nuget, $ProductName)
Copy-Recurse -Path $AssemblyPath -Destination $BuildFull -Include *.nupkg -Overwrite $false
Copy-Recurse -Path $AssemblyPath -Destination $PathFull -Include *.nupkg -Overwrite $false
Add-NuGet -Path $Lib -NuSpec $NuGetSpecFile

# NuGet: Framework.Web.Core
$AssemblyPath=[String]::Format("{0}\*.Web.*", $Lib)
$NuGetSpecFile = [String]::Format("{0}\{1}.Web.Core.nuspec", $Nuget, $ProductName)
Copy-Recurse -Path $AssemblyPath -Destination $BuildFull -Include *.nupkg -Overwrite $false
Copy-Recurse -Path $AssemblyPath -Destination $PathFull -Include *.nupkg -Overwrite $false
Add-NuGet -Path $Lib -NuSpec $NuGetSpecFile

# NuGet: Framework.Web.Full
$AssemblyPath=[String]::Format("{0}\*.Web.*", $Lib)
$NuGetSpecFile = [String]::Format("{0}\{1}.Web.Full.nuspec", $Nuget, $ProductName)
Copy-Recurse -Path $AssemblyPath -Destination $BuildFull -Include *.nupkg -Overwrite $false
Copy-Recurse -Path $AssemblyPath -Destination $PathFull -Include *.nupkg -Overwrite $false
Add-NuGet -Path $Lib -NuSpec $NuGetSpecFile

# NuGet: Framework.Windows
$AssemblyPath=[String]::Format("{0}\*.Windows.*", $Lib)
$NuGetSpecFile = [String]::Format("{0}\{1}.Windows.nuspec", $Nuget, $ProductName)
Copy-Recurse -Path $AssemblyPath -Destination $BuildFull -Include *.nupkg -Overwrite $false
Copy-Recurse -Path $AssemblyPath -Destination $PathFull -Include *.nupkg -Overwrite $false
Add-NuGet -Path $Lib -NuSpec $NuGetSpecFile

# NuGet: Framework.Universal
$AssemblyPath=[String]::Format("{0}\*.Universal.*", $Lib)
$NuGetSpecFile = [String]::Format("{0}\{1}.Universal.nuspec", $Nuget, $ProductName)
Copy-Recurse -Path $AssemblyPath -Destination $BuildFull -Include *.nupkg -Overwrite $false
Copy-Recurse -Path $AssemblyPath -Destination $PathFull -Include *.nupkg -Overwrite $false
Add-NuGet -Path $Lib -NuSpec $NuGetSpecFile
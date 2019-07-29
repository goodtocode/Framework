#-----------------------------------------------------------------------
# <copyright file="Framework-for-WPF-Shopping-Cart.ps1" company="Genesys Source">
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
	[String]$Path = '\\Dev-Web-01.dev.genesyssource.com',
	[String]$Build = '\\Dev-Vm-01.dev.genesyssource.com\Vault\Builds\Sprints',
	[String]$Domain = 'www.genesyssource.com',
	[String]$Database = 'DatabaseServer.dev.genesyssource.com',	
	[String]$ProductName = 'Framework-for-Core',
	[String]$RepoName = 'Genesys-Framework',
	[String]$SubFolder = 'docs.genesyssource.com',
	[String]$Relative='..\..\',
	[String]$SolutionFolder = 'Quick-Start'
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
Import-Module ($Relative + "Build.Scripts.Modules\Code\Genesys.Code.psm1")
Import-Module ($Relative + "Build.Scripts.Modules\System\Genesys.System.psm1")

# ***
# *** Validate and cleanse
# ***
$Path = Set-Unc -Path $Path
$Build = Set-Unc -Path $Build

# ***
# *** Locals
# ***
$BuildSprint = [String]::Format("{0}\{1}\{2}", $Build, (Get-Date).ToString("yyyy.MM"), $SubFolder)
$BuildFull = [String]::Format("{0}\{1}", $BuildSprint, $ProductName)
$PathFull = [String]::Format("{0}\Sites\{1}\{2}", $Path, $SubFolder, $RepoName)

# ***
Write-Verbose "Builds"
# ***
Copy-SourceCode -Path $BuildFull -RepoName $RepoName -ProductName $ProductName -SolutionFolder $SolutionFolder -Snk GenesysFramework.snk

# ***
Write-Verbose "Zip"
# ***
Remove-Path -Path "$BuildFull\src\Framework.Android"
Remove-Path -Path "$BuildFull\src\Framework.DataAccess.Full"
Remove-Path -Path "$BuildFull\src\Framework.DesktopApp"
Remove-Path -Path "$BuildFull\src\Framework.Interop.Portable"
Remove-Path -Path "$BuildFull\src\Framework.iOS"
Remove-Path -Path "$BuildFull\src\Framework.Models.Portable"
Remove-Path -Path "$BuildFull\src\Framework.Test.Full"
Remove-Path -Path "$BuildFull\src\Framework.UniversalApp.Full"
Remove-Path -Path "$BuildFull\src\Framework.WebApp.Full"
Remove-Path -Path "$BuildFull\src\Framework.WebServices.Full"
Clear-Solution -Path $BuildFull -Include *.snk, *.log, *.txt, *.bak, *.tmp, *.vspscc, *.vssscc, *.csproj.vspscc, *.sqlproj.vspscc, *.cache, __*.PNG, *.vstemplate
Copy-File -Path ("$BuildSprint\Genesys-Extensions.zip") -Destination $BuildFull
Copy-File -Path ("$BuildSprint\Genesys-Framework.zip") -Destination $BuildFull
Compress-Solution -Path $BuildFull -Destination $BuildSprint -RepoName $RepoName -File ("Genesys-$ProductName.zip")


# ***
Write-Verbose "Publish"
# ***
Copy-File -Path ("$BuildSprint\Genesys-$ProductName.zip") -Destination $PathFull
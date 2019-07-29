
#-----------------------------------------------------------------------
# <copyright file="Framework-Shopping-Cart.ps1" company="Genesys Source">
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
	[String]$Domain = 'code.genesyssource.com',
	[String]$Database = 'DatabaseServer.dev.genesyssource.com',	
	[String]$ProductName = 'Extensions',
	[String]$RepoName = 'Genesys-Extensions',
	[String]$SubFolder = 'docs.genesyssource.com',
	[String]$Relative='..\..\',
	[String]$SolutionFolder = ''
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
$ZipName = "$RepoName.zip"
$BuildSprint = [String]::Format("{0}\{1}\{2}", $Build, (Get-Date).ToString("yyyy.MM"), $SubFolder)
$BuildFull = [String]::Format("{0}\{1}", $BuildSprint, $RepoName)
$PathFull = [String]::Format("{0}\Sites\{1}\{2}", $Path, $SubFolder, $RepoName)

# ***
Write-Verbose "Builds"
# ***
Copy-SourceCode -Path $BuildFull -RepoName $RepoName -ProductName $ProductName -Snk GenesysFramework.snk

# ***
Write-Verbose "Zip"
# ***
Clear-Solution -Path $BuildFull -Include *.snk, *.log, *.txt, *.bak, *.tmp, *.vspscc, *.vssscc, *.csproj.vspscc, *.sqlproj.vspscc, *.cache, __*.PNG, *.vstemplate
Compress-Solution -Path $BuildFull -Destination $BuildSprint -RepoName $RepoName -File $ZipName

# ***
Write-Verbose "Publish"
# ***
Copy-File -Path ("$BuildSprint\$ZipName") -Destination $PathFull

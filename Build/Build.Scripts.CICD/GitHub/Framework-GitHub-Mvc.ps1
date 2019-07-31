#-----------------------------------------------------------------------
# <copyright file="Framework-GitHub-MVC.ps1" company="GoodToCode Source">
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
	[String]$RepoName = 'GoodToCode-Framework',
	[String]$ProductName = 'Framework-for-MVC',
	[String]$SubFolder = 'GitHub',
	[String]$Database = 'DatabaseServer.dev.GoodToCode.com',
	[String]$Build = '\\Dev-Vm-01.dev.GoodToCode.com\Vault\builds\sprints',
	[String]$Lib='\lib',
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
Import-Module ($Relative + "Build.Scripts.Modules\Code\GoodToCode.Code.psm1")
Import-Module ($Relative + "Build.Scripts.Modules\System\GoodToCode.System.psm1")

# ***
# *** Validate and cleanse
# ***
$Path = Set-Unc -Path $Path
$Build = Set-Unc -Path $Build

# ***
# *** Locals
# ***
$PathFull = [String]::Format("{0}\{1}\{2}", $Path, $SubFolder, $ProductName)
$BuildFull = [String]::Format("{0}\{1}\{2}\{3}", $Build, (Get-Date).ToString("yyyy.MM"), $SubFolder, $ProductName)
$GitHub = [string]::Format("{0}..\Build\Build.Content\GitHub\{1}", $Relative, $ProductName)
[String]$Src=Convert-PathSafe -Path ([String]::Format("{0}..\{1}", $Relative, $SolutionFolder))
[String]$BuildSrc=[String]::Format("{0}\src", $BuildFull)
[String]$BuildLib=[String]::Format("{0}\lib", $BuildFull)
[String]$BuildSolution=[String]::Format("{0}\{1}.sln", $BuildSrc, $ProductName)

# ***
Write-Verbose "Builds"
# ***
New-Path -Path $BuildFull -Clean $True
Copy-SourceCode -Path $BuildFull -RepoName $RepoName -ProductName $ProductName -SubFolder $SubFolder -SolutionFolder $SolutionFolder -Snk GoodToCode.snk -Relative ($Relative+'..\')
Copy-Recurse -Path $GitHub -Destination $BuildFull
Remove-Path -Path "$BuildFull\src\Framework.Android"
Remove-Path -Path "$BuildFull\src\Framework.DesktopApp"
Remove-Path -Path "$BuildFull\src\Framework.iOS"
Remove-Path -Path "$BuildFull\src\Framework.UniversalApp.Full"
Remove-Path -Path "$BuildFull\src\Framework.UniversalApp.Core"
Remove-Path -Path "$BuildFull\src\Framework.WebServices.Core"
Remove-Path -Path "$BuildFull\src\Framework.WebServices.Full"

# ***
Write-Verbose "Publish"
# ***
Copy-Recurse -Path $BuildFull -Destination $PathFull
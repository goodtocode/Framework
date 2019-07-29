#-----------------------------------------------------------------------
# <copyright file="ListFiles.ps1" company="Genesys Source">
#      Copyright (c) Genesys Source. All rights reserved.
#      All rights are reserved. Reproduction or transmission in whole or in part, in
#      any form or by any means, electronic, mechanical or otherwise, is prohibited
#      without the prior written consent of the copyright owner.
# </copyright>
# Example: ListFiles.ps1 -Path 'C:\Source\Framework\3.00-Alpha\Vsix\Vsix.Universal\ProjectTemplates\UWP\Framework.UniversalApp'
#-----------------------------------------------------------------------
# ***
# *** Parameters
# ***
param(
	[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 	[string]$Path = $(throw '-Source is a required parameter.'),
	[String]$Relative = '..\..\',
	[string[]]$Include = "*.*",
 	[string[]]$Exclude = "",
	[Int32]$First = 1000
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

# ***
# *** Locals
# ***

Get-Childitem -Path $Path -Include $Include -Exclude $Exclude -Recurse | % {
     Write-Host $_.FullName.Length - $_.FullName
}
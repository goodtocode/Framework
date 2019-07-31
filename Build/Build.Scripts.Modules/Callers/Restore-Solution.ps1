#-----------------------------------------------------------------------
# <copyright file="Restore-Solution.ps1" company="GoodToCode Source">
#      Copyright (c) GoodToCode Source. All rights reserved.
#      All rights are reserved. Reproduction or transmission in whole or in part, in
#      any form or by any means, electronic, mechanical or otherwise, is prohibited
#      without the prior written consent of the copyright owner.
# </copyright>
#-----------------------------------------------------------------------

# ***
# *** Parameters
# ***
param (
	[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 	[string]$Path = $(throw '-Path is a required parameter.'),
	[string]$Build = 'Rebuild',
	[string]$Configuration = 'Release',
	[string]$Framework = 'v4.6',
	[string]$Version = '15.0',
	[String]$Relative='..\..\'
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
Write-Host "*** Starting: $ThisScript On: $(Get-Date)"
Write-Host "*****************************"
# Imports
Import-Module "..\..\Build.Scripts.Modules\Code\GoodToCode.Code.psm1"
Import-Module "..\..\Build.Scripts.Modules\System\GoodToCode.System.psm1"

# ***
# *** Execute
# ***
Restore-Solution -Path $Path -Build $Build -Configuration $Configuration -Framework $Framework -Version $Version
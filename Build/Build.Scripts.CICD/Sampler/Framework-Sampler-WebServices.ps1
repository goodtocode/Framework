#-----------------------------------------------------------------------
# <copyright file="Framework-Sampler-Web.ps1" company="GoodToCode Source">
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
	[String]$Path=$(throw '-Path is a required parameter.'),
	[String]$Destination = '\\Dev-Web-01.dev.GoodToCode.com', 
	[String]$Domain = 'sampler.GoodToCode.com',
	[String]$Database = 'DatabaseServer.dev.GoodToCode.com',
	[String]$Build = '\\Dev-Vm-01.dev.GoodToCode.com\Vault\builds\sprints',	
	[String]$RepoName = 'GoodToCode-Framework',
	[String]$Relative = "..\..\"
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
$Destination = Set-Unc -Path $Destination 
$Build = Set-Unc -Path $Build

# ***
# *** Execute
# ***
# WebServices
[String]$PathFull = ($Path + '\_PublishedSites\Framework.WebServices.Full')
[String]$DestinationFull = [String]::Format("{0}\Sites\{1}\{2}-for-WebApi", $Destination, $Domain, $RepoName)
Copy-WebSite -Path $PathFull -Destination $DestinationFull

# ***
# *** Finalize
# ***
# Display Errors
if ($Error.count -ne 0) {
	for ($i=$Error.Count-1; $i -ge 0;$i--) {
		@($Error[$i].Exception.Message) | out-file "$ThisDir\log.txt" -append
	}
}

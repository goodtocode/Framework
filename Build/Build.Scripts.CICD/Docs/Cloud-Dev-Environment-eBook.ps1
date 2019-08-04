#-----------------------------------------------------------------------
# <copyright file="Cloud-Dev-Environment-eBook.ps1" company="GoodToCode">
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
	[String]$Path = '\\Dev-Vm-01.dev.GoodToCode.com\Vault\Drops', 
	[String]$Destination = '\\Dev-Web-01.dev.GoodToCode.com', 
	[String]$Domain = 'docs.GoodToCode.com',
	[String]$Database = 'DatabaseServer.dev.GoodToCode.com',
	[String]$Build = '\\Dev-Vm-01.dev.GoodToCode.com\Vault\builds\sprints',	
	[String]$ProductName = 'Cloud-Dev-Environment',
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
# *** Locals
# ***
[String]$eBook = [String]::Format("{0}-eBook.pdf", $ProductName)
[String]$PathFull = [String]::Format("{0}\Sites\{1}\Library\{2}", $Path, $Domain, $ProductName)
[String]$DestinationFull = [String]::Format("{0}\Sites\{1}\Library\{2}", $Destination, $Domain, $ProductName)
[String]$BuildSprint = [String]::Format("{0}\{1}\{2}", $Build, (Get-Date).ToString("yyyy.MM"), $Domain)
[String]$BuildFull = [String]::Format("{0}\Library\{1}", $BuildSprint, $ProductName)

# ***
Write-Verbose "Builds"
# ***
Copy-File -Path ($PathFull + "\" + $eBook) -Destination $BuildFull

# ***
Write-Verbose "Publish"
# ***
Copy-File -Path ($BuildFull + "\" + $eBook) -Destination $DestinationFull

# ***
# *** Finalize
# ***
# Display Errors
if ($Error.count -ne 0) {
	for ($i=$Error.Count-1; $i -ge 0;$i--) {
		@($Error[$i].Exception.Message) | out-file "$ThisDir\log.txt" -append
	}
}

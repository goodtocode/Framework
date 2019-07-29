#-----------------------------------------------------------------------
# <copyright file="Extensions-Docs.ps1" company="Genesys Source">
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
	[String]$Domain = 'docs.genesyssource.com',
	[String]$Database = 'DatabaseServer.dev.genesyssource.com'
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
Import-Module "..\..\Build.Scripts.Modules\Code\Genesys.Code.psm1"
Import-Module "..\..\Build.Scripts.Modules\System\Genesys.System.psm1"

# ***
# *** Validate and cleanse
# ***
$Path = Set-Unc -Path $Path


# ***
# *** Locals
# ***
[String]$Solution="..\..\..\Docs\Extensions.Docs.sln"
[String]$Source="..\..\..\Docs\Extensions.Docs\Genesys-Extensions"
$Path=[String]::Format("{0}\{1}", $Path, "\Sites\docs.GenesysSource.com\Reference\Genesys-Extensions")

# ***
# *** Execute
# ***
Restore-Solution -Path $Solution
Copy-WebSite -Path $Source -Destination $Path


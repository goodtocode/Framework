#-----------------------------------------------------------------------
# <copyright file="GoodToCode.Code.psm1" company="GoodToCode">
#      Copyright (c) GoodToCode. All rights reserved.
#      All rights are reserved. Reproduction or transmission in whole or in part, in
#      any form or by any means, electronic, mechanical or otherwise, is prohibited
#      without the prior written consent of the copyright owner.
# </copyright>
#-----------------------------------------------------------------------

#-----------------------------------------------------------------------
# Add-CopyrightApache [-Path [<String>]]
#
# Example: .\Add-CopyrightApache -Path \\source\path\MyFramework.sln
#-----------------------------------------------------------------------
function Add-CopyrightApache
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 		[string]$Path = $(throw '-Path is a required parameter.'),
 		[string[]]$Include = "*.cs",
 		[string[]]$Exclude = "",
		[Int32]$First = 100

		)
	$Path = Set-Unc -Path $Path
	# *** Change Destination code Headers
	[String]$OldLicense1 = '//      Copyright (c) GoodToCode. All rights reserved.'
	[String]$OldLicense2 = '//      All rights are reserved. Reproduction or transmission in whole or in part, in'
	[String]$OldLicense3 = '//      any form or by any means, electronic, mechanical or otherwise, is prohibited'
	[String]$OldLicense4 = '//      without the prior written consent of the copyright owner.'
	[String]$NewLicense1 = '//      '
	[String]$NewLicense2 = '//      Licensed to the Apache Software Foundation (ASF) under one or more ' + [Environment]::NewLine `
		+ '//      contributor license agreements.  See the NOTICE file distributed with ' + [Environment]::NewLine `
		+ '//      this work for additional information regarding copyright ownership.' + [Environment]::NewLine `
		+ '//      The ASF licenses this file to You under the Apache License, Version 2.0 ' + [Environment]::NewLine `
		+ '//      (the ''License''); you may not use this file except in compliance with '
	[String]$NewLicense3 = '//      the License.  You may obtain a copy of the License at ' + [Environment]::NewLine `
		+ '//       ' + [Environment]::NewLine `
		+ '//        http://www.apache.org/licenses/LICENSE-2.0 ' + [Environment]::NewLine `
		+ '//       ' + [Environment]::NewLine `
		+ '//       Unless required by applicable law or agreed to in writing, software  '
	[String]$NewLicense4 = '//       distributed under the License is distributed on an ''AS IS'' BASIS, ' + [Environment]::NewLine `
		+ '//       WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  ' + [Environment]::NewLine `
		+ '//       See the License for the specific language governing permissions and  ' + [Environment]::NewLine `
		+ '//       limitations under the License. '
	if (test-folder -Path $Path) {
		# Get all children in a folder
		$CodeFiles=get-childitem  -Path $Path -Include $Include  -Recurse -Force | select -First $First
		$Affected = 0
		# Iterate through child files
		foreach ($Item in $CodeFiles)
		{
			# Change file contents
			(Get-Content $Item.PSPath) | 
			Foreach-Object {$_-replace $OldLicense1, $NewLicense1 -replace $OldLicense2, $NewLicense2 -replace $OldLicense3, $NewLicense3 -replace $OldLicense4, $NewLicense4
			} | 
			Set-Content $Item.PSPath
			$Affected = $Affected + 1
		}
		Write-Verbose "[Success] $Affected items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
	else
	{
		Write-Host "[OK] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path does not exist."
	}
}
export-modulemember -function Add-CopyrightApache

#-----------------------------------------------------------------------
# Add-NuGet [-Path [<String>]]
#
# Example: .\Add-NuGet -Path \Lib\Framework\*.Models.* -NuSpec \\Build\Build.Content\NuGet\MyPackage.nuspec
#-----------------------------------------------------------------------
function Add-NuGet
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 		[string]$Path = $(throw '-Path is a required parameter.'),
		[string]$NuSpec = $(throw '-NuSpec is a required parameter.'),
		[string]$Key = '2FA2DC63-0510-46A7-BFBF-0AA72F1EB453',
		[string]$Url = '\\Dev-Web-01.dev.goodtocode.com\Sites\nuget.goodtocode.com\Packages'
	)
	Write-Host "Add-NuGet -Path $Path -NuSpec $NuSpec"
	[String]$NuGetExe = '..\..\..\Build\Build.Content\Utility\NuGet\NuGet.exe'
	$Path = Set-Unc -Path $Path	
	Write-Verbose "$NuGetExe pack $NuSpec -BasePath $Path -Verbosity detailed"
	& $NuGetExe pack "$NuSpec" -BasePath "$Path" -Verbosity detailed
	Write-Verbose "$NuGetExe push *.nupkg -Source $Url -Verbosity detailed"
	& $NuGetExe push *.nupkg $Key -Source "$Url" -Verbosity detailed
}
export-modulemember -function Add-NuGet

#-----------------------------------------------------------------------
# Clear-Solution [-Path [<String>]]
#                  [-Include [<String[]>] [-Exclude [<String[]>]]
#
# Example: .\Clear-Solution \\source\path
#-----------------------------------------------------------------------
function Clear-Solution
{
	param (
	 [Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 	 [string]$Path = $(throw '-Path is a required parameter.'),
 	 [string[]]$Include = ("*.snk", "*.zip", "*.log", "*.bak", "*.tmp,  *.vspscc", "*.vssscc", "*.csproj.vspscc", "*.sqlproj.vspscc", "*.cache"),
 	 [string]$Exclude = ""
	)
	Write-Host "Clear-Solution -Path $Path -Include $Include -Exclude $Exclude"
	$Path = Set-Unc -Path $Path
	# Cleanup Files
	Remove-Recurse -Path $Path -Include $Include -Exclude $Exclude
	# Cleanup Folders
	Remove-Subfolders -Path $Path -Subfolder "TestResults"
	Remove-Subfolders -Path $Path -Subfolder ".vs"
	Remove-Subfolders -Path $Path -Subfolder "packages"
	Remove-Subfolders -Path $Path -Subfolder "bin"
	Remove-Subfolders -Path $Path -Subfolder "obj"
}
export-modulemember -function Clear-Solution

#-----------------------------------------------------------------------
# Clear-Lib [-Path [<String>]]
#                  [-Include [<String[]>] [-Exclude [<String[]>]]
#
# Example: .\Clear-Lib \\source\path
#-----------------------------------------------------------------------
function Clear-Lib
{
	param (
	 [Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 	 [string]$Path = $(throw '-Path is a required parameter.'),
 	 [string[]]$Include = ("*.snk", "*.zip", "*.log", "*.bak", "*.tmp,  *.vspscc", "*.vssscc", "*.csproj.vspscc", "*.sqlproj.vspscc", "*.cache"),
 	 [string]$Exclude = ""
	)
	Write-Host "Clear-Lib -Path $Path -Include $Include -Exclude $Exclude"
	$Path = Set-Unc -Path $Path
	$Path = Add-Prefix -String $Path -Add "\\"
	# Cleanup Files
	Remove-Recurse -Path $Path -Include $Include -Exclude $Exclude
	# Cleanup Folders
	Remove-Subfolders -Path $Path -Subfolder "TestResults"
	Remove-Subfolders -Path $Path -Subfolder ".vs"
	Remove-Subfolders -Path $Path -Subfolder "packages"
	Remove-Subfolders -Path $Path -Subfolder "bin"
	Remove-Subfolders -Path $Path -Subfolder "obj"
	Remove-Subfolders -Path $Path -Subfolder "app_data"
}
export-modulemember -function Clear-Lib

#-----------------------------------------------------------------------
# Compress-Solution [-Path [<String>]] [-Destination [<String>]] 
#                 	[-Include [<String[]>] [-Exclude [<String[]>]]
#					[-RepoName [<String>]] [-File [<String>]]
#
# Example: .\Compress-Solution \\source\path \\destination\path
#-----------------------------------------------------------------------
function Compress-Solution
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 		[string]$Path=$(throw '-Path is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 		[string]$Destination=$(throw '-Destination is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[String]$RepoName=$(throw '-RepoName is a required parameter.'),		
		[String]$Lib=("\lib"),
		[String]$Build = '',
		[String]$File = ''
	)
	Write-Host "Compress-Solution -Path $Path -Destination $Destination -RepoName $RepoName -Build $Build -File $File"
	# ***
	# *** Validate and cleanse
	# ***
	$Path = Set-Unc -Path $Path
	$Destination = Set-Unc -Path $Destination 
	
	# ***
	# *** Locals
	# ***
	if(-not($File -and $File.Contains(".zip"))) { $File = [String]::Format("{0}.zip", $RepoName) }
	$ZipFile = [string]::Format("{0}\{1}", $Destination, $File)
	Clear-Solution -Path ($Path + '\src\')
	Compress-Path -Path $Path -File $ZipFile
}
export-modulemember -function Compress-Solution

#-----------------------------------------------------------------------
# Get-Version [-Major [<String>]] [-Minor [<String>]]] [-Revision [<String>]]] [-Build [<String>]]]
#  - Convention: Major is any int16. Minor is year based. Revision is month based. Build is hour based.
# Example: .\Get-Version -Major 4
#-----------------------------------------------------------------------
function Get-Version
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[String]$Major = $(throw '-Major is a required parameter.'),
		[String]$Minor = '',
		[String]$Revision = '',
		[String]$Build = '',
		[String]$Format = 'M.YY.MM.HHH'
	)	
	Write-Host "Get-Version -Major $Major -Minor $Minor -Revision $Revision -Build $Build"
	[DateTime]$Now = Get-Date
	[DateTime]$BoM = Get-Date -Year $Now.Year -Month $Now.Month -Day 1 -Hour 0 -Minute 0 -Second 0
	[String]$returnValue = ''

	$Minor = $Minor.Replace('-1', '')
	$Revision = $Revision.Replace('-1', '')
	$Build = $Build.Replace('-1', '')
	$TimeSpan = $Now - $Bom
	$HoursSoFar = ($TimeSpan.Days * 24) + $TimeSpan.Hours;
	[String] $YY = $Now.Year.ToString().Substring(2, 2).PadLeft(2, "0")
	[String] $MM = $Now.Month.ToString().PadLeft(2, "0")
	# Default: M.YYYY.MM.HHH
	if($Minor -eq '') { $Minor = $YY}
	if($Revision -eq '') { $Revision = "$YY$MM" }
	if($Build -eq '') { 
		$Build = [String]::Format("{0}{1}1", $HoursSoFar.ToString().PadLeft(4, "0"), $Now.Minute.ToString().PadLeft(2, "0")).ToString().PadLeft(6, "0") 
		if (Compare-IsLast -String $Build -EndsWith '0'){
			Remove-Suffix -String $Build -Remove '0'
			Add-Suffix -String $Build -Add '1'
		}
	}
	$returnValue = [String]::Format("{0}.{1}.{2}.{3}", $Major, $Minor, $Revision, $Build)
	# Override default if need
	if($Format -eq 'M.YY.MM')
	{
		$returnValue = [String]::Format("{0}.{1}.{2}", $Major, $Minor, $Revision)
	}
	elseif($Format -eq 'YYYY.MM')
	{
		$returnValue = [String]::Format("{0}.{1}", $Now.Year, $Revision)
	}
	
	return $returnValue
}
export-modulemember -function Get-Version

#-----------------------------------------------------------------------
# Set-Version [-Path [<String>]]
#                  [-Contains [<String[]>] [-Close [<String[]>]]
#
# Example: .\Set-Version -Path \\source\path
#-----------------------------------------------------------------------
function Set-Version
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 		[string]$Path = $(throw '-Path is a required parameter.'),
		[String]$Major = '4',
		[String]$Minor = '19',
		[String]$Revision = '',
		[String]$Build = ''
	)
	Write-Host "Set-Version -Path $Path"
	# .Net Projects
	$Version = Get-Version -Major $Major -Minor $Minor -Revision $Revision -Build $Build
	Update-ContentsByTag -Path $Path -Value $Version -Open '<version>' -Close '</version>' -Include *.nuspec
	Update-LineByContains -Path $Path -Contains "AssemblyVersion(" -Line "[assembly: AssemblyVersion(""$Version"")]" -Include AssemblyInfo.cs
	# Vsix Templates
	$OldVersion = Get-Version -Major $Major -Minor ($Now.Month - 1).ToString("00") -Format 'M.YY.MM'
	$Version = Get-Version -Major $Major -Format 'M.YY.MM.HHH'
	Update-Text -Path $Path -Old '4.19.01' -New $Version -Include *.vsixmanifest
	# No NuGet Version Needed - handled by that individual process
}
export-modulemember -function Set-Version

#-----------------------------------------------------------------------
# Copy-FrameworkRepo [-String [<String>]]
#
# Example: .\Copy-FrameworkRepo -Path "\\Dev-Vm-01.dev.goodtocode.com\Vault\GitHub\Extensions" -Repo "Extensions"
#	Result: false
#-----------------------------------------------------------------------
function Copy-FrameworkRepo
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[String]$Path = $(throw '-Path is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 		[String]$Repo = $(throw '-Repo is a required parameter.')
		
	)
	Write-Host "Copy-FrameworkRepo -Repo $Repo"	
	$Path = Set-Unc -Path $Path
	# Compile
	[String]$SolutionFile=[String]::Format("{0}.sln", $Repo)
	[String]$Solution=[String]::Format("{0}\{1}", $PathCode, $SolutionFile)
	Restore-Solution -Path $Solution

	# *** Copy GitHub Root Files
	[String]$PathGitHubFiles=[string]::Format("{0}\Repo-{1}", $GitHubFolder, $Repo)
	Redo-Path -Path $PathGitHubFiles -Destination $Path 
	# *** Copy \lib files
	[String]$PathLib="\lib"
	[String]$DestinationLib=[String]::Format("{0}\lib", $Path)
	Redo-Path -Path $PathLib -Destination $DestinationLib
	# *** Copy \src files
	[String]$DestinationCode=[String]::Format("{0}\src", $Path)	
	[String]$PathCode=[String]::Format("..\..\..\{0}", $Repo)
	Redo-Path -Path $PathCode -Destination $DestinationCode -Exclude *.snk, *.log, *.txt, *.bak, *.tmp, *.vspscc, *.vssscc, *.csproj.vspscc, *.sqlproj.vspscc, *.cache	
	
	# Cleanup Files
	Clear-Solution -Path $DestinationCode -Include *.sln, *.snk, *.log, *.txt, *.bak, *.tmp, *.vspscc, *.vssscc, *.csproj.vspscc, *.sqlproj.vspscc, *.cache, __*.PNG, *.vstemplate -Exclude $SolutionFile
	# Special-copy solution file
	Copy-File -Path $Solution -Destination $DestinationCode
}
export-modulemember -function Copy-FrameworkRepo

#-----------------------------------------------------------------------
# Copy-FrameworkTemplate [-String [<String>]]
#
# Example: .\Copy-FrameworkTemplate -Path "\\Dev-Vm-01.dev.goodtocode.com\Vault\GitHub\Extensions" -Repo "Extensions"
#	Result: false
#-----------------------------------------------------------------------
function Copy-FrameworkTemplate
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[String]$Path = $(throw '-Path is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 		[String]$Repo = $(throw '-Repo is a required parameter.'),
		[String[]]$Exclude = ("*.snk", "*.sln", "*.log", "*.txt", "*.bak", "*.tmp", "*.vspscc", "*.vssscc", "*.csproj.vspscc", "*.sqlproj.vspscc", "*.cache", "__*.PNG", "*.vstemplate")
	)
	Write-Host "Copy-FrameworkTemplate -Repo $Repo"
	$Path = Set-Unc -Path $Path
	# Compile
	[String]$SolutionFile=[String]::Format("{0}.sln", $Repo)
	[String]$Solution=[String]::Format("{0}\{1}", $PathCode, $SolutionFile)
	Restore-Solution -Path $Solution	
	# *** Copy GitHub Root Files
	[String]$PathGitHubFiles=[string]::Format("{0}\Repo-{1}", $GitHubFolder, $Repo)
	Redo-Path -Path $PathGitHubFiles -Destination $Path 
	# *** Copy \lib files
	[String]$PathLib="\lib"
	[String]$DestinationLib=[String]::Format("{0}\lib", $Path)
	Redo-Path -Path $PathLib -Destination $DestinationLib
	# *** Copy \src files
	[String]$DestinationCode=[String]::Format("{0}\src", $Path)	
	[String]$PathCode=[String]::Format("..\..\..\{0}", $Repo)
	Redo-Path -Path $PathCode -Destination $DestinationCode -Exclude $Exclude	
	
	# Cleanup Files
	Clear-Solution -Path $DestinationCode -Include $Exclude -Exclude $SolutionFile
	# Special-copy solution file
	Copy-File -Path $Solution -Destination $DestinationCode
}
export-modulemember -function Copy-FrameworkTemplate

#-----------------------------------------------------------------------
# Copy-WebSite [-Path [<String>]]
#
# Example: .\Copy-WebSite -Path \\Build\Site -Destination \\Drops\Site
#	Result: false
#-----------------------------------------------------------------------
function Copy-WebSite
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[String]$Path = $(throw '-Path is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[String]$Destination = $(throw '-Destination is a required parameter.'),
		[bool]$Clean = $False
	)	
	Write-Host "Copy-WebSite -Path $Path -Destination $Destination"
	$Path = Set-Unc -Path $Path
	if (test-folder -Path $Path)
	{
		# Optionally clean
		if($Clean -eq $True) { Remove-Path -Path $Destination }
		Remove-Recurse -Path ($Destination + "\obj")
		Remove-Recurse -Path ($Destination + "\testresults")
		New-Path -Path $Destination
		Disable-WebSite -Path $Destination
		Copy-Recurse -Path $Path -Destination $Destination
		Enable-WebSite -Path $Destination
	}
}
export-modulemember -function Copy-WebSite

#-----------------------------------------------------------------------
# Disable-WebSite [-Path [<String>]]
#
# Example: .\Disable-WebSite -Path \\Build\Site -Destination \\Drops\Site
#	Result: false
#-----------------------------------------------------------------------
function Disable-WebSite
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[String]$Path = $(throw '-Path is a required parameter.')
	)	
	Write-Host "Disable-WebSite -Path $Path"
	[String]$App_OfflineRelative="..\..\..\Build\Build.Content\Admin\app_offline.htm"
	$Path = Set-Unc -Path $Path
	if (test-folder -Path $Path)
	{
		# Go off-line		
		Write-Verbose "Convert-PathSafe -Path $App_OfflineRelative"
		$AppOfflineAbsolute = Convert-PathSafe -Path $App_OfflineRelative
		Write-Verbose "AppOfflineAbsolute: $AppOfflineAbsolute"
		Copy-File -Path $AppOfflineAbsolute -Destination $Path
	}
}
export-modulemember -function Disable-WebSite

#-----------------------------------------------------------------------
# Enable-WebSite [-Path [<String>]]
#
# Example: .\Enable-WebSite -Path \\Build\Site -Destination \\Drops\Site
#	Result: false
#-----------------------------------------------------------------------
function Enable-WebSite
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[String]$Path = $(throw '-Path is a required parameter.')
	)	
	Write-Host "Enable-WebSite -Path $Path"
	$Path = Set-Unc -Path $Path
	if (test-folder -Path $Path)
	{
		# Go on-line
		Remove-File -Path ($Path + "\app_offline.htm")
	}
}
export-modulemember -function Enable-WebSite

#-----------------------------------------------------------------------
# Find-MsBuild [-Path [<String>]]
#
# Example: .\Find-MsBuild
#	Result: 
#-----------------------------------------------------------------------
function Find-DevEnv
{
	param (
		[int] $Year = 2017
	)
	Write-Host "Find-DevEnv -Year $Year"
	$ExeName = 'DevEnv.exe'
	[int] $FolderYear = 2017;
	if($Year -gt 2016) {$FolderYear = $Year}
    $devPath = "$Env:programfiles (x86)\Microsoft Visual Studio\$FolderYear\Enterprise\Common7\IDE\$ExeName"
    $proPath = "$Env:programfiles (x86)\Microsoft Visual Studio\$FolderYear\Professional\Common7\IDE\$ExeName"
    $communityPath = "$Env:programfiles (x86)\Microsoft Visual Studio\$FolderYear\Community\Common7\IDE\$ExeName"
    $fallback2015Path = "${Env:ProgramFiles(x86)}\Microsoft Visual Studio 14.0\Common7\IDE\$ExeName"
    $fallback2013Path = "${Env:ProgramFiles(x86)}\Microsoft Visual Studio 12.0\Common7\IDE\$ExeName"
	$fallback2010Path = "${Env:ProgramFiles(x86)}\Microsoft Visual Studio 10.0\Common7\IDE\$ExeName"
		
    If ((2017 -le $Year) -And (Test-Path $devPath)) { return $devPath } 
    If ((2017 -le $Year) -And (Test-Path $proPath)) { return $proPath } 
    If ((2017 -le $Year) -And (Test-Path $communityPath)) { return $communityPath } 
    If ((2015 -le $Year) -And (Test-Path $fallback2015Path)) { return $fallback2015Path } 
    If ((2013 -le $Year) -And (Test-Path $fallback2013Path)) { return $fallback2013Path } 
	If ((2010 -le $Year) -And (Test-Path $fallback2010Path)) { return $fallback2010Path } 
}
export-modulemember -function  Find-DevEnv

#-----------------------------------------------------------------------
# Find-MsBuild [-Path [<String>]]
#
# Example: .\Find-MsBuild
#	Result: 
#-----------------------------------------------------------------------
function Find-MsBuild
{
	param (
		[int]$Year = 2017,
		[string]$Version = '15.0'
	)
	Write-Host "Find-MsBuild -Year $Year - Version $Version"
	$ExeName = 'MsBuild.exe'
	[int] $FolderYear = 2017;
	if($Year -gt 2016) {$FolderYear = $Year}
    $agentPath = "$Env:programfiles (x86)\Microsoft Visual Studio\$FolderYear\BuildTools\MSBuild\$Version\Bin\$ExeName"
    $devPath = "$Env:programfiles (x86)\Microsoft Visual Studio\$FolderYear\Enterprise\MSBuild\$Version\Bin\$ExeName"
    $proPath = "$Env:programfiles (x86)\Microsoft Visual Studio\$FolderYear\Professional\MSBuild\$Version\Bin\$ExeName"
    $communityPath = "$Env:programfiles (x86)\Microsoft Visual Studio\$FolderYear\Community\MSBuild\$Version\Bin\$ExeName"
    $fallback2015Path = "${Env:ProgramFiles(x86)}\MSBuild\14.0\Bin\$ExeName"
    $fallback2013Path = "${Env:ProgramFiles(x86)}\MSBuild\12.0\Bin\$ExeName"
    $fallbackPath = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\$ExeName"
		
    If ((2017 -le $Year) -And (Test-Path $agentPath)) { return $agentPath } 
    If ((2017 -le $Year) -And (Test-Path $devPath)) { return $devPath } 
    If ((2017 -le $Year) -And (Test-Path $proPath)) { return $proPath } 
    If ((2017 -le $Year) -And (Test-Path $communityPath)) { return $communityPath } 
    If ((2015 -le $Year) -And (Test-Path $fallback2015Path)) { return $fallback2015Path } 
    If ((2013 -le $Year) -And (Test-Path $fallback2013Path)) { return $fallback2013Path } 
    If (Test-Path $fallbackPath) { return $fallbackPath } 
}
export-modulemember -function  Find-MsBuild

#-----------------------------------------------------------------------
# Copy-SourceCode [-Path [<String>]] [-Destination [<String>]]
#
# Example: .\Copy-SourceCode -Path \\Build\Site -Destination \\Drops\Site
#-----------------------------------------------------------------------
function Copy-SourceCode
{
param(
	[String]$Path = '\\Dev-Vm-01.dev.goodtocode.com\Vault\builds\sprints',
	[String]$RepoName = 'GoodToCode-Extensions',
	[String]$ProductName = 'Extensions',
	[String]$Lib='',
	[String]$Relative = '..\..\..\',
	[String]$SolutionFolder = '',
	[String]$Snk='GoodToCodeFramework.snk'
)
	Write-Host "Copy-SourceCode -Path $Path -RepoName $RepoName -ProductName $ProductName -Snk $Snk -Relative $Relative -Clean $Clean"
	# Cleanse Variables
	$Path = Set-Unc -Path $Path
	# Locals
	if ($SolutionFolder) { $Src=Convert-PathSafe -Path ([String]::Format("{0}{1}", $Relative, $SolutionFolder)) }
	else { $Src=Convert-PathSafe -Path ([String]::Format("{0}{1}", $Relative, $ProductName)) }
	$Solution=[string]::Format("{0}\{1}.sln", $Src, $ProductName)
	$PathSrc=[String]::Format("{0}\src", $Path)
	$PathLib=[String]::Format("{0}\lib", $Path)
	$PathSolution=[String]::Format("{0}\{1}.sln", $PathSrc, $ProductName)

	# Copy
	if($Lib){ Copy-Recurse -Path $Lib -Destination $PathLib }
	else { New-Path -Path $PathLib }
	Copy-Recurse -Path $Src -Destination $PathSrc -Exclude __*.png, *.vstemplate, *.sln, *.snk, *.log, *.txt, *.bak, *.tmp, *.vspscc, *.vssscc, *.csproj.vspscc, *.sqlproj.vspscc, *.cache
	Clear-Solution -Path $PathSrc
	# Solution File - Copy and cleanse
	Copy-File -Path $Solution -Destination $PathSrc
	Remove-TFSBinding -Path $PathSrc
	Remove-StrongNameKey -Path $PathSrc -File $Snk
}
export-modulemember -function Copy-SourceCode

#-----------------------------------------------------------------------
# Remove-StrongNameKey [-Path [<String>]]
#
# Example: .\Remove-StrongNameKey -Path "C:\Source\My Clones\Extensions-master\Extensions-master\src\Extensions\Extensions.Full"
#-----------------------------------------------------------------------
function Remove-StrongNameKey
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 		[string]$Path = $(throw '-Path is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$File = $(throw '-File is a required parameter.')
		)
	Write-Host "Remove-StrongNameKey -Path $Path -File $File"
	$Path = Set-Unc -Path $Path
	
	# Change SNK from true to false
	Update-Text -Path $Path -Include *.csproj -Old '<SignAssembly>true</SignAssembly>' -New '<SignAssembly>false</SignAssembly>'	
	
	# Remove SNK originator reference
	#  <PropertyGroup>
	#		<AssemblyOriginatorKeyFile>GoodToCodeFramework.snk</AssemblyOriginatorKeyFile>
	#  </PropertyGroup>	
	Remove-ContentsByTagContains -Path $Path -Include *.csproj -Open "<PropertyGroup>" -Close "</PropertyGroup>" -Contains $File
	
	# Remove SNK file reference
	#	<ItemGroup>
	#		<None Include="MySNKFile.snk" />
	#		<None Include="packages.config" />
	#		...
	#	</ItemGroup>
	# Or
	#  <ItemGroup>
	#    <None Include="MySNKFile.snk" />
	#  </ItemGroup>
	Remove-ContentsByTagContains -Path $Path -Include *.csproj -Open "<ItemGroup>" -Close "</ItemGroup>" -Contains $File
}
export-modulemember -function Remove-StrongNameKey

#-----------------------------------------------------------------------
# Remove-Subdomain [-Domain [<String>]]
#
# Example: .\Remove-Subdomain -Domain www.goodtocode.com
#	Result: goodtocode.com
#-----------------------------------------------------------------------
function Remove-Subdomain
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 		[string]$Domain = $(throw '-Domain is a required parameter.')
	)
	Write-Host "Remove-Subdomain -Domain $Domain"
	[string]$ReturnValue = $Domain
	[Int]$Period1 = -1
	[Int]$Period2 = -1
	
	$Period1 = $Domain.IndexOf('.')	
	if($Period1 -gt 0)
	{
		$Period2 = $Domain.IndexOf('.', $Period1)
		if(($Period1 -le $Period2) -and ($Period1 -le $Domain.Length))
		{
			$ReturnValue = $Domain.Substring($Period1 + 1)
			Write-Verbose "[Success] 1 items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
		}
	}

	if($Period2 -le $Period1)
	{
		Write-Host "[OK] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). Not a Uri domain part format"
	}

	return $ReturnValue
}
export-modulemember -function Remove-Subdomain

#-----------------------------------------------------------------------
# Remove-TFSBinding [-Path [<String>]]
#
# Example: .\Remove-TFSBinding -Path \\source\path
#-----------------------------------------------------------------------
function Remove-TFSBinding
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 		[string]$Path = $(throw '-Path is a required parameter.')
		)
	Write-Host "Remove-TFSBinding -Path $Path"
	$Path = Set-Unc -Path $Path
	Remove-ContentsByTag -Path $Path -Include *.sln -Open "GlobalSection(TeamFoundationVersionControl) = preSolution" -Close "EndGlobalSection"	
}
export-modulemember -function Remove-TFSBinding

#-----------------------------------------------------------------------
# Update-AppSetting [-Path [<String>]]
#
# Example: .\Update-AppSetting -Path \\source\path -Key "MyWebService" -Value "http://sampler.goodtocode.com/GoodToCode-framework-for-webapi/v1"
#-----------------------------------------------------------------------
function Update-AppSetting
{
	param (
		[Parameter(Mandatory=$True,ValueFromPipelineByPropertyName=$True)]
 		[string]$Path = $(throw '-Path is a required parameter.'),
		[string]$File = 'AppSettings.config',
		[string]$Key = $(throw '-Key is a required parameter.'),
		[string]$Value = $(throw '-Value is a required parameter.')
	)
	Write-Host "Update-AppSetting -Path $Path -Key $Key -Value $Value"
	$Path = Set-Unc -Path $Path
	$Contains = [string]::Format('key="{0}"', $Key)
	$Mask = '<add key="{0}" value="{1}" />'
	$New = [String]::Format($Mask, $Key, $Value)
	Update-LineByContains -Path $Path -Contains $Contains -Line $New -Include $File
}
export-modulemember -function Update-AppSetting
  
#-----------------------------------------------------------------------
# Update-ConnectionString [-Path [<String>]]
#
# Example:
#	SQL Express .mdf file: .\Update-ConnectionString -Path \\source\path -Key "DefaultConnection" -Value "Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\App_Data\FrameworkData.mdf;Integrated Security=True;Application Name=GoodToCodeFramework;" -Provider "System.Data.SqlClient"
#	SQL Server ADO.NET: .\Update-ConnectionString -Path $BuildFull -Key "DefaultConnection" -Value "data source=DatabaseServer.dev.goodtocode.com;initial catalog=FrameworkData;integrated security=True;application name=GoodToCodeFramework;" -Provider "System.Data.SqlClient"
#	SQL SErver EF: <add name="TestADOConnection" connectionString="Data Source=DatabaseServer.dev.goodtocode.com;Initial Catalog=FrameworkData;Connect Timeout=180;Application Name=GoodToCodeFramework;" -Provider "System.Data.EntityClient"
#-----------------------------------------------------------------------
function Update-ConnectionString
{
	param (
		[Parameter(Mandatory=$True,ValueFromPipelineByPropertyName=$True)]
 		[string]$Path = $(throw '-Path is a required parameter.'),
		[string]$File = 'ConnectionStrings.config',
		[string]$Key = $(throw '-Key is a required parameter.'),
		[string]$Value = $(throw '-Value is a required parameter.'),
		[string]$Provider = 'System.Data.SqlClient'
	)
	Write-Host "Update-ConnectionString -Path $Path -Key $Key -Value $Value"
	$Path = Set-Unc -Path $Path
	$Contains = [string]::Format('name="{0}"', $Key)
	$Mask = '<add name="{0}" connectionString="{1}" providerName="{2}" />'
	$New = [String]::Format($Mask, $Key, $Value, $Provider)
	Update-LineByContains -Path $Path -Contains $Contains -Line $New -Include $File
}
export-modulemember -function Update-ConnectionString

#-----------------------------------------------------------------------
# Get-TFSLatest [-Path [<String>]] [-Destination [<String>]] 
#                 	[-Include [<String[]>] [-Exclude [<String[]>]]
#					[-ProductName [<String>]] [-ProductNameFull [<String>]]
#
# Example: .\Compress-Solution \\source\path \\destination\path
#-----------------------------------------------------------------------
function Get-TFSLatest
{
	param( 
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path = $(throw '-Path is a required parameter.'),
		[string]$Server = "http://GoodToCode.visualstudio.com",
		[string]$Include = "*.*",
 		[string]$Exclude = ""
	)
	Write-Host "Get-TFSLatest -Path $Path -Server $Server -Include $Include -Exclude $Exclude"
	$Path = Set-Unc -Path $Path
	if ( (Get-PSSnapin -Name Microsoft.TeamFoundation.PowerShell -ErrorAction SilentlyContinue) -eq $null )
	{
		Add-PSSnapin Microsoft.TeamFoundation.PowerShell
	}

	$TfsServer = Get-TfsServer -name $Path

	$TfExePath = "${env:ProgramFiles(x86)}\Microsoft Visual Studio 10.0\Common7\IDE\TF.exe"

	foreach ($item in Get-TfsChildItem $ServerBranchLocation -r -server $tfs) 
	{ 
		if (($item -match $Include) -and ($item -notmatch $Exclude))
		{ 
			& "$TFExePath" get $item.ServerItem /force /noprompt
		}
	}
}
export-modulemember -function Get-TFSLatest

#-----------------------------------------------------------------------
# Restore-Brochure [-RepoName [<String>]]
#
# Example: Restore-Brochure -RepoName 'Common-Business' -DocName 'Why-Reusability'
#-----------------------------------------------------------------------
function Restore-Brochure
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$RepoName = $(throw '-RepoName is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$BrochureName = $(throw '-BrochureName is a required parameter.'),
 		[string]$Path = '\\dev-ro-xviii.dev.goodtocode.com\c$\Users\rjgood\GoodToCode\GoodToCode - Docs',		
		[String]$Destination = '\\Dev-Web-01.dev.goodtocode.com\Sites',
		[string]$AdditionalFile = ''
	)
	Write-Host "Restore-Brochure -RepoName $RepoName -BrochureName $BrochureName"
	$Path = Set-Unc -Path $Path
	$Source=[String]::Format("{0}\Brochures\{1}", $Path, $RepoName)
	$Destination=[String]::Format("{0}\docs.goodtocode.com\Brochures\{1}", $Destination, $RepoName)
	Write-Verbose "Path: $Path, RepoName: $RepoName, Source: $Source, Destination: $Destination"
	# Brochure
	$DocPath = [String]::Format("{0}\{1}\{1}.pdf", $Source, $BrochureName)
	Copy-File -Path $DocPath -Destination $Destination
	# Additional file
	if(-not ($AdditionalFile -eq '')) {
		$DocPath = [String]::Format("{0}\{1}\{2}", $Source, $BrochureName, $AdditionalFile)
		Copy-File -Path $DocPath -Destination $Destination
	}
}
export-modulemember -function Restore-Brochure

#-----------------------------------------------------------------------
# Restore-ProductDoc [-RepoName [<String>]]
#
# Example: Restore-ProductDoc -RepoName 'Common-Business' -DocName 'Why-Reusability'
#-----------------------------------------------------------------------
function Restore-ProductDoc
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$RepoName = $(throw '-RepoName is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$DocName = $(throw '-BrochureName is a required parameter.'),
 		[string]$Path = '\\dev-ro-xviii.dev.goodtocode.com\c$\Users\rjgood\GoodToCode\GoodToCode - Docs',		
		[String]$Destination = '\\Dev-Web-01.dev.goodtocode.com\Sites',
		[string]$AdditionalFile = ''
	)
	Write-Host "Restore-ProductDoc -RepoName $RepoName -BrochureName $DocName"
	$Path = Set-Unc -Path $Path
	$Source=[String]::Format("{0}\Products\{1}", $Path, $RepoName)
	$Destination=[String]::Format("{0}\docs.goodtocode.com\Products\{1}", $Destination, $RepoName)
	Write-Verbose "Path: $Path, RepoName: $RepoName, Source: $Source, Destination: $Destination"
	# Brochure
	$DocPath = [String]::Format("{0}\{1}\{1}.pdf", $Source, $DocName)
	Copy-File -Path $DocPath -Destination $Destination
	# Additional file
	if(-not ($AdditionalFile -eq '')) {
		$DocPath = [String]::Format("{0}\{1}\{2}", $Source, $DocName, $AdditionalFile)
		Copy-File -Path $DocPath -Destination $Destination
	}
}
export-modulemember -function Restore-ProductDoc

#-----------------------------------------------------------------------
# Restore-NuGet [-Path [<String>]]
#
# Example: .\Restore-NuGet -Path \\source\path\MyFramework.sln
# Cli: https://docs.microsoft.com/en-us/visualstudio/msbuild/msbuild-command-line-reference
# Versions: https://docs.microsoft.com/en-us/visualstudio/msbuild/msbuild-toolset-toolsversion
#-----------------------------------------------------------------------
function Restore-NuGet
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 		[string]$Path = $(throw '-Path is a required parameter.'),
		[String]$ConfigFile = '..\..\..\Build\.nuget\NuGet.config',
		[String]$NuGetExe = '..\..\..\Build\Build.Content\Utility\NuGet\NuGet.exe'
	)
	Write-Host "Restore-NuGet -Path $Path"
	$Path = Set-Unc -Path $Path	
	Write-Verbose "$NuGetExe -ConfigFile $ConfigFile"
	& $NuGetExe restore "$Path" -ConfigFile "$ConfigFile"	
}
export-modulemember -function Restore-NuGet

#-----------------------------------------------------------------------
# Restore-VMDocsVMDocs [-RepoName [<String>]]
#
# Example: Restore-VMDocs -RepoName 'GoodToCode-Cloud-Dev-Environment'
#-----------------------------------------------------------------------
function Restore-VMDocs
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$RepoName = $(throw '-Path is a required parameter.'),
 		[string]$Path = '\\dev-ro-xviii.dev.goodtocode.com\c$\Users\rjgood\GoodToCode\GoodToCode - Docs',
		[String]$Build = '\\Dev-Vm-01.dev.goodtocode.com\Vault\Builds\Sprints',
		[string]$Folder = 'docs.goodtocode.com'
	)
	Write-Host "Restore-VMDocs -RepoName $RepoName -Path $Path -Build $Build -AdditionalFile $AdditionalFile"
	$Path = Set-Unc -Path $Path
	$Build = [String]::Format("{0}\{1}\docs.goodtocode.com\{2}", $Build, (Get-Date).ToString("yyyy.MM"), $RepoName)	
	$Path=[String]::Format("{0}\Products\{1}", $Path, $RepoName)
	Write-Verbose "After Transformation - Path: $Path, Build: $Build"

	# ***
	# *** Build
	# ***
	# Quick-Guide
	$DocPath = [String]::Format("{0}\{1}-Quick-Guide\{1}-Quick-Guide.pdf", $Path, $RepoName)
	Copy-File -Path $DocPath -Destination $Build		
	$DocPath = [String]::Format("{0}\{1}-Quick-Guide\{1}-Quick-Guide.png", $Path, $RepoName)	
	Copy-File -Path $DocPath -Destination $Build
	# Requirements
	$DocPath = [String]::Format("{0}\{1}-Requirements\{1}-Requirements.pdf", $Path, $RepoName)
	Copy-File -Path $DocPath -Destination $Build
	$DocPath = [String]::Format("{0}\{1}-Requirements\{1}-Requirements.png", $Path, $RepoName)
	Copy-File -Path $DocPath -Destination $Build
	# Your
	$DocPath = [String]::Format("{0}\Your-{1}\Your-{1}.pdf", $Path, $RepoName)
	Copy-File -Path $DocPath -Destination $Build
	$DocPath = [String]::Format("{0}\Your-{1}\Your-{1}.png", $Path, $RepoName)
	Copy-File -Path $DocPath -Destination $Build
	# Additional File
	if(-not ($AdditionalFile -eq '')) {
		$DocPath = [String]::Format("{0}\{1}\{2}", $Path, $RepoName, $AdditionalFile)
		Copy-File -Path $DocPath -Destination $Build
	}
}
export-modulemember -function Restore-VMDocs

#-----------------------------------------------------------------------
# Restore-Solution [-Path [<String>]]
#
# Example: .\Restore-Solution -Path \\source\path\MyFramework.sln
# Cli: https://docs.microsoft.com/en-us/visualstudio/msbuild/msbuild-command-line-reference
# Versions: https://docs.microsoft.com/en-us/visualstudio/msbuild/msbuild-toolset-toolsversion
#-----------------------------------------------------------------------
function Restore-Solution
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 		[string]$Path = $(throw '-Path is a required parameter.'),
		[string]$Build = 'Rebuild',
		[string]$Configuration = 'Release',		
		[String]$Relative='..\..\..\',
		[Boolean]$DevEnv = $True
	)
	Write-Host "Restore-Solution -Path $Path -Build $Build -Configuration $Configuration -Framework $Framework -Version $Version"		
	[String]$NuGetExe = (Set-Unc ($Relative + 'Build\Build.Content\Utility\NuGet')) + '\NuGet.exe'
	$Path = Set-Unc -Path $Path
	Set-Version -Path $Path
	Write-Verbose "$NuGetExe restore $Path -source https://api.nuget.org/v3/index.json;"
	& $NuGetExe restore "$Path"

	if($DevEnv -eq $True) {
		[String]$DevenvExe = Find-DevEnv
		Write-Host "$DevenvExe $Path /Rebuild"
		& $DevenvExe $Path /Rebuild
		Write-Host "Build Complete"
	}
	else {
		[String]$MsBuildExe = Find-MsBuild
		Write-Host "$MsBuildExe $Path /t:$Build /tv:$Version /p:TargetFramework=$Framework /p:Configuration=$Configuration"
		& $MsBuildExe $Path /t:$Build /p:Configuration=$Configuration
	}
}
export-modulemember -function Restore-Solution

#-----------------------------------------------------------------------
# Restore-VsixTemplate [-String [<String>]]
#
# Example: .\Restore-VsixTemplate -Path "\\Dev-Vm-01.dev.goodtocode.com\Vault\GitHub\Extensions" -Repo "Extensions"
#	Result: false
#-----------------------------------------------------------------------
function Restore-VsixTemplate
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[String]$Path = $(throw '-Path is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[String]$Destination = $(throw '-Destination is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[String]$Build = $(throw '-Build is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[String]$ProductFlavor = $(throw '-ProductFlavor is a required parameter.'),
		[String]$Database = 'DatabaseServer.dev.goodtocode.com',
		[String]$RepoName="GoodToCode-Framework",
		[String]$FamilyName="Framework",
		[String]$SolutionFolder="Quick-Start"
	)

	Write-Host "Restore-VsixTemplate -Path $Path -Destination $Destination -Build $Build -ProductFlavor $ProductFlavor -Database $Database -RepoName $RepoName -FamilyName $FamilyName -ProductName $SolutionFolder"
	# ***
	# *** Validate and cleanse
	# ***
	$Path = Set-Unc -Path $Path
	$Build = Set-Unc -Path $Build
	$Destination = Set-Unc -Path $Destination

	# Locals
	[String]$LibPath="\Lib"
	[String]$VsixPath="..\..\..\Vsix"
	[String]$ContentPath="..\..\..\Build\Build.Content"
	# Staging
	[String]$BuildZipPath=[String]::Format("{0}\ToZip", $Build)
	[String]$BuildZipFile=[string]::Format("{0}\{1}.zip", $Build, $ProductFlavor)
	# Vsix
	[String]$VstemplateFile = [String]::Format("{0}.vstemplate", $ProductFlavor)	
	[String]$VsixSolutionFile=[string]::Format("{0}\Vsix.{1}.sln", $VsixPath, $ProductFlavor)
	[String]$VsixProjectTemplateZip=[string]::Format("{0}\Vsix.{1}\ProjectTemplates\{1}.zip", $VsixPath, $ProductFlavor)
	
	#
	# Build quick-start Zip
	#
	Restore-Solution -Path $VsixSolutionFile # Must rebuild first
	New-Path -Path $BuildZipPath -Clean $True
	Copy-Recurse -Path $Path -Destination $BuildZipPath -Exclude *.dbmdl, *.jfm
	Clear-Solution -Path $BuildZipPath
	Remove-Recurse -Path $BuildZipPath -Include *.sln, *.vstemplate
	Copy-Recurse -Path ("$ContentPath\Vsix") -Destination $BuildZipPath -Include *.vstemplate
	Copy-File -Path ("$ContentPath\Vsix\__PreviewImage.png") -Destination $BuildZipPath
	Copy-File -Path ("$ContentPath\Vsix\__TemplateIcon$ProductFlavor.png") -Destination $BuildZipPath
	Rename-File -Path ("$BuildZipPath\$ProductFlavor.vstemplate") -NewName root.bak
	Remove-File -Path ("$BuildZipPath\*.vstemplate")
	Rename-File -Path ("$BuildZipPath\root.bak") -NewName root.vstemplate
	if ($ProductFlavor -eq 'Core') {
			Remove-Path -Path "$BuildZipPath\Framework.Android"
			Remove-Path -Path "$BuildZipPath\Framework.DataAccess.Full"
			Remove-Path -Path "$BuildZipPath\Framework.DesktopApp"
			Remove-Path -Path "$BuildZipPath\Framework.Interop.Portable"
			Remove-Path -Path "$BuildZipPath\Framework.iOS"
			Remove-Path -Path "$BuildZipPath\Framework.Models.Portable"
			Remove-Path -Path "$BuildZipPath\Framework.Test.Full"
			Remove-Path -Path "$BuildZipPath\Framework.UniversalApp.Full"
			Remove-Path -Path "$BuildZipPath\Framework.WebApp.Full"
			Remove-Path -Path "$BuildZipPath\Framework.WebServices.Full"
		}
	if ($ProductFlavor -eq 'MVC') {
			Remove-Path -Path "$BuildZipPath\Framework.Android"
			Remove-Path -Path "$BuildZipPath\Framework.DesktopApp"
			Remove-Path -Path "$BuildZipPath\Framework.iOS"
			Remove-Path -Path "$BuildZipPath\Framework.UniversalApp.Full"
			Remove-Path -Path "$BuildZipPath\Framework.UniversalApp.Core"
			Remove-Path -Path "$BuildZipPath\Framework.WebServices.Core"
			Remove-Path -Path "$BuildZipPath\Framework.WebServices.Full"
		}
		if ($ProductFlavor -eq 'UWP') {
			Remove-Path -Path "$BuildZipPath\Framework.Android"
			Remove-Path -Path "$BuildZipPath\Framework.DesktopApp"
			Remove-Path -Path "$BuildZipPath\Framework.iOS"
			Remove-Path -Path "$BuildZipPath\Framework.WebApp.Core"
			Remove-Path -Path "$BuildZipPath\Framework.WebApp.Full"
	}
	if ($ProductFlavor -eq 'WebAPI') {
			Remove-Path -Path "$BuildZipPath\Framework.Android"
			Remove-Path -Path "$BuildZipPath\Framework.DesktopApp"
			Remove-Path -Path "$BuildZipPath\Framework.iOS"
			Remove-Path -Path "$BuildZipPath\Framework.UniversalApp.Full"
			Remove-Path -Path "$BuildZipPath\Framework.UniversalApp.Core"
			Remove-Path -Path "$BuildZipPath\Framework.WebApp.Core"
			Remove-Path -Path "$BuildZipPath\Framework.WebApp.Full"
	}
	if ($ProductFlavor -eq 'WPF') {
			Remove-Path -Path "$BuildZipPath\Framework.Android"
			Remove-Path -Path "$BuildZipPath\Framework.iOS"
			Remove-Path -Path "$BuildZipPath\Framework.UniversalApp.Full"
			Remove-Path -Path "$BuildZipPath\Framework.UniversalApp.Core"
			Remove-Path -Path "$BuildZipPath\Framework.WebApp.Core"
			Remove-Path -Path "$BuildZipPath\Framework.WebApp.Full"
	}
	# Fix: Dependencies won't load unless we change ..\packages to ..\..\packages. NuGet and VSIX want solution folder in different levels, so we compensate for VSIX
	Update-Text -Path $Build -Include *.csproj -Old '..\packages' -New '..\..\packages' 
	Compress-Path -Path $BuildZipPath -File $BuildZipFile

	#
	# Build VSIX
	#
	Set-ItemProperty $VsixProjectTemplateZip -name IsReadOnly -value $false
	Copy-File -Path $BuildZipFile -Destination $VsixProjectTemplateZip
	Restore-Solution -Path $VsixSolutionFile -DevEnv $False

	#
	# Publish
	#
	Copy-Recurse -Path ("$LibPath\Vsix-for-$ProductFlavor") -Destination $Destination
}
export-modulemember -function Restore-VsixTemplate
#-----------------------------------------------------------------------
#-----------------------------------------------------------------------
#
# File: Compress-Template.ps1
#
#-----------------------------------------------------------------------
#-----------------------------------------------------------------------
# ***
# *** Parameters
# ***
param(
	[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 	[string]$Path = $(throw '-Path is a required parameter.'),
	[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
	[String]$ZipName = $(throw '-ZipName is a required parameter.'),
	[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
	[String]$Destination = $(throw '-Destination is a required parameter.')
)

#-----------------------------------------------------------------------
# Compare-IsLast [-String [<String>]]
#
# Example: .\Compare-IsLast -String Hell -EndsWith H
#	Result: false
# Example: .\Compare-IsLast -String Hello -Add H
#	Result: Hello
#-----------------------------------------------------------------------
function Compare-IsLast
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 		[string]$String = $(throw '-String is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$EndsWith = $(throw '-EndsWith is a required parameter.')
	)
	Write-Verbose "Compare-IsLast -String $String -EndsWith $EndsWith"
	[Boolean]$ReturnValue = $false
	if($EndsWith.Length -lt $String.Length)
	{
		$StringEnding = $String.SubString(($String.Length - $EndsWith.Length), $EndsWith.Length)
		if ($StringEnding.ToLower().Equals($EndsWith.ToLower()))
		{ 		
			$ReturnValue = $true
		}
		Write-Verbose "[Success] 1 items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
	else
	{
		Write-Verbose "[OK] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}

	return $ReturnValue
}

#-----------------------------------------------------------------------
# Compress-Path [-Path [<String>]] [-File [<String>]] 
#
# Example: .\Compress-Path \\source\path \\destination\path\file.zip
#-----------------------------------------------------------------------
function Compress-Path
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 		[string]$Path=$(throw '-Path is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 		[string]$File=$(throw '-File is a required parameter.')
	)
	Write-Verbose "Compress-Path -Path $Path -File $File"
	New-Path -Path $Path
	Remove-File $File
	[Reflection.Assembly]::LoadWithPartialName("System.IO.Compression.FileSystem")
	[System.IO.Compression.ZipFile]::CreateFromDirectory($Path, $File)
}

#-----------------------------------------------------------------------
# Convert-PathSafe [-Path [<String>]]
#
# Example: .\Convert-PathSafe -Path \\source\path
#-----------------------------------------------------------------------
function Convert-PathSafe
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 		[string]$Path = $(throw '-Path is a required parameter.')
	)
	Write-Verbose "Convert-PathSafe -Path $Path"
	$Path = $Path.Trim()
	$ReturnValue = $Path
	$Path = Set-Unc -Path $Path
	if(Test-Path -Path $Path)
	{
		$ReturnValue = Convert-Path -Path $Path
		if (-not ($ReturnValue))
		{
			$ReturnValue = $Path
			Write-Verbose "[Warning] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path didnt convert."
		}
	}
	else
	{
		Write-Host "[Error] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path does not exist."		
	}
	return $ReturnValue
}

#-----------------------------------------------------------------------
# Copy-File [-Path [<String>]] [-Destination [<String>]]
#
# Example: .\Copy-File -Path \\source\path\File.name -Destination \\destination\path
#-----------------------------------------------------------------------
function Copy-File
{
	param (
		[Parameter(Mandatory = $True)]
 		[string]$Path = $(throw '-Path is a required parameter.'),
		[Parameter(Mandatory = $True)]
 		[string]$Destination = $(throw '-Destination is a required parameter.'),
		[string[]]$Include = "*.*",
 		[string[]]$Exclude = "",
		[bool]$Overwrite = $true
	)
	Write-Verbose "Copy-File -Path $Path -Destination $Destination -Overwrite $Overwrite"
	$Destination = Set-Unc -Path $Destination
	if(Test-File -Path $Path)
	{
		New-Path -Path $Destination
		$DestinationAbsolute = $Destination		
		if(Test-Folder -Path $Destination)
		{
			$DestinationAbsolute = Convert-PathSafe -Path $Destination	
		}
		$DestinationPathFile = $DestinationAbsolute
		$FolderArray = $Path.Split('\')
		if($FolderArray.Count -gt 0)
		{
			$DestinationPathFile = Join-Path $DestinationAbsolute $FolderArray[$FolderArray.Count-1]
		}
		if((-not (Test-Path $DestinationPathFile -PathType Leaf)) -or ($Overwrite -eq $true))
		{
			try{
				Copy-Item -Path $Path -Destination $DestinationAbsolute -Include $Include -Exclude $Exclude -Force
			}
			catch{
				Write-Host "[Error] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path does not exist."
			}			
		}
		Write-Verbose "[Success] 1 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path to -Destination $DestinationAbsolute"
	}
	else
	{
		Write-Host "[Error] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path does not exist."
	}
}

#-----------------------------------------------------------------------
# Get-CurrentLine
#
# Example: Get-CurrentLine
#-----------------------------------------------------------------------
function Get-CurrentLine { 
    $MyInvocation.ScriptLineNumber 
} 

#-----------------------------------------------------------------------
# Get-CurrentFile
#
# Example: Get-CurrentFile
#-----------------------------------------------------------------------
function Get-CurrentFile { 
    $MyInvocation.ScriptName 
} 

#-----------------------------------------------------------------------
# New-Path [-Path [<String>]]
#
# Example: .\New-Path \\source\path
#-----------------------------------------------------------------------
function New-Path
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 		[string]$Path = $(throw '-Path is a required parameter.'),
		[bool]$Clean=$false
	)
	Write-Verbose "New-Path -Path $Path"
	[String]$Folder = ""
	$Path = Remove-Suffix -String $Path -Remove "\"	
	if ($Clean) {Remove-Path -Path $Path}
	if (-not (test-path $Path)) {
		if (Test-Unc $Path)
		{
			$PathArray = $Path.Split('\')
			foreach($item in $PathArray)
			{
				if($item.Length -gt 0)
				{
					if($Folder.Length -lt 1)
					{
						$Folder = "\\$item"
					}
					else
					{
						$Folder = "$Folder\$item"
						if (-not (Test-Path $Folder)) {
							New-Item -ItemType directory -Path $Folder -Force
						}
					}
				}
			}
		}
		else
		{
				New-Item -ItemType directory -Path $Path -Force
		}
	}
}

#-----------------------------------------------------------------------
# Test-Unc [-Path [<String>]]
#
#
# Example: .\Test-Unc -Path \\source\path
#-----------------------------------------------------------------------
function Test-Unc
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 		[string]$Path = $(throw '-Path is a required parameter.')
	)
	Write-Verbose "Test-Unc -Path $Path"
	[bool]$ReturnValue = $false
	if(($Path.Contains('\\')) -and (-not ($Path.Contains(':\'))))
	{
		$ReturnValue = $true
	}
	else
	{
		Write-Verbose "[OK] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
	return $ReturnValue
}

#-----------------------------------------------------------------------
# Remove-File [-File [<String>]]
#
# Example: .\Remove-File \\source\path\file.txt
#-----------------------------------------------------------------------
function Remove-File
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 		[string]$Path=$(throw '-Path is a required parameter.')
	)
	Write-Verbose "Remove-File -Path $Path"
	if (Test-File -Path $Path) { 
		Remove-Item -Path $Path -Force 		
		Write-Verbose "[Success] 1 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path removed."
	}
	else
	{
		Write-Host "[Error] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path does not exist."
	}
}

#-----------------------------------------------------------------------
# Remove-Suffix [-String [<String>]]
#
# Example: .\Remove-Suffix -String Hell -Remove o
#	Result: Hello
#-----------------------------------------------------------------------
function Remove-Suffix
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 		[string]$String = $(throw '-String is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Remove = $(throw '-Remove is a required parameter.')
	)
	Write-Verbose "Remove-Suffix -String $String -Remove $Remove"
	[string]$ReturnValue = $String
	if($String)
	{
		if (Compare-IsLast -String $String -EndsWith $Remove)
		{ 		
			$ReturnValue = $ReturnValue.Substring(0, $String.Length - $Remove.Length)
		}
		Write-Verbose "[Success] 1 items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
	else
	{
		Write-Verbose "[OK] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -String $String already has suffix of -Remove $Remove"
	}

	return $ReturnValue
}

#-----------------------------------------------------------------------
# Set-Unc [-Path [<String>]]
#
# Example: .\Set-Unc -Path \\source\path
#-----------------------------------------------------------------------
function Set-Unc
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 		[string]$Path = $(throw '-Path is a required parameter.')
	)
	Write-Verbose "Set-Unc -Path $Path"
	$Path = $Path.Trim()
	$Path = Remove-Suffix -String $Path -Remove '\'
	if(-not ($Path.Contains(':\') -or $Path.Contains('.\') -or (Compare-IsFirst -String $Path -BeginsWith '\')))
	{
		$ReturnValue = Add-Prefix -String $Path -Add '\\'
	}
	else
	{
		$ReturnValue = $Path
		Write-Verbose "[OK] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path already a UNC, drive letter, absolute or relative path."
	}
	return $ReturnValue
}

#-----------------------------------------------------------------------
# Test-File [-Path [<String>]]
#
# Example: .\Test-File -Path \\source\path
#-----------------------------------------------------------------------
function Test-File
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 		[string]$Path = $(throw '-Path is a required parameter.')
	)
	Write-Verbose "Test-File -Path $Path"
	[bool]$ReturnValue = $false
	if(Test-Path -Path $Path -PathType Leaf)
	{
		$ReturnValue = $true
	}
	else
	{
		Write-Verbose "[Warning] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path does not exist or is not a File."
	}
	return $ReturnValue
}

#-----------------------------------------------------------------------
# Test-Folder [-Path [<String>]]
#
#
# Example: .\Test-Folder -Path \\source\path
#-----------------------------------------------------------------------
function Test-Folder
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 		[string]$Path = $(throw '-Path is a required parameter.')
	)
	Write-Verbose "Test-Folder -Path $Path"
	[bool]$ReturnValue = $false
	if(test-path -Path $Path -PathType Container)
	{
		$ReturnValue = $true
	}	
	else
	{
		Write-Verbose "[Warning] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path does not exist or is not a Folder."
	}
	return $ReturnValue
}

# ***
# *** Initialize
# ***
Set-ExecutionPolicy Unrestricted -Scope Process -Force
$VerbosePreference = 'SilentlyContinue' #'SilentlyContinue' # 'Continue'
[String]$ThisScript = $MyInvocation.MyCommand.Path
[String]$ThisDir = Split-Path $ThisScript
Set-Location $ThisDir # Ensure our location is correct, so we can use relative paths
Write-Host "*****************************"
Write-Host "*** Starting: $ThisScript on $(Get-Date -format 'u')"
Write-Host "*****************************"

# Initialize
$PathFull = Convert-PathSafe -Path $Path
$PathFull = Set-UNC $PathFull
$DestinationFull = Convert-PathSafe -Path $Destination
$DestinationFull = Set-UNC $DestinationFull  
$ZipPathFile = Remove-Suffix -String $ThisDir -Remove "\App_Data"
$ZipPathFile = "$ZipPathFile\bin\debug\$ZipName"

Write-Host "Path: $Path"
Write-Host "PathFull: $PathFull"
Write-Host "ZipName: $ZipName"
Write-Host "Destination: $Destination"
Write-Host "DestinationFull: $DestinationFull"
Write-Host "ZipPathFile: $ZipPathFile"

Remove-File -Path $ZipPathFile
Compress-Path -Path $PathFull -File $ZipName
Copy-File -Path $ZipPathFile -Destination $DestinationFull
Remove-File -Path $ZipPathFile

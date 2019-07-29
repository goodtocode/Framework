# Parameters
param([String]$Path=".\")

# Initialize
#Set-ExecutionPolicy Unrestricted -Scope Process -Force
[String]$ThisScript = $MyInvocation.MyCommand.Path
[String]$ThisDir = Split-Path $ThisScript
Set-Location $ThisDir # Ensure our location is correct, so we can use relative paths

# Initialize
$Path=$Path.Replace("`"","")

# Relay parameter to output
Write-Output "Path: $Path"

# Add override to allow partial class extension of the EF generated files
$configFiles=get-childitem -Path "$Path\*.cs"
foreach ($file in $configFiles)
{
	(Get-Content $file.PSPath) | 
	Foreach-Object {$_-replace "public System.Guid Id", "public override System.Guid Id" `
		-replace "public int Id", "public override int Id" `
		-replace "public System.Guid Key", "public override System.Guid Key" `
		-replace "public byte", "public override byte" `
		-replace "public int RecordStatus", "public override System.Guid RecordStatus" `
		-replace "public int ActivityContextID { get; set; }", "public override int ActivityContextID { get; set; }" `
		-replace "public System.DateTime CreatedDate { get; set; }", "public override System.DateTime CreatedDate { get; set; }" `
		-replace "public System.DateTime ModifiedDate { get; set; }", "public override System.DateTime ModifiedDate { get; set; }"
	} | 
	Set-Content $file.PSPath
}
#
# ConfigureWebServer.ps1
#
Configuration Main
 {
     Node ('localhost')
     {
         WindowsFeature WebServerRole
         {
             Name = "Web-Server"
             Ensure = "Present"
         }

         WindowsFeature WebAspNet45
         {
             Name = "Web-Asp-Net45"
             Ensure = "Present"
             Source = $Source
             DependsOn = "[WindowsFeature]WebServerRole"
         }

         #script block to download WebPI MSI from the Azure storage blob
         Script DownloadWebPIImage
         {
             GetScript = {
                 @{
                     Result = "WebPIInstall"
                 }
             }

             TestScript = {
                 Test-Path "C:\temp\wpilauncher.exe"
             }

             SetScript ={
                 $source = "http://go.microsoft.com/fwlink/?LinkId=255386"
                 $destination = "C:\temp\wpilauncher.exe"
                 Invoke-WebRequest $source -OutFile $destination
             }
         }

         Package WebPi_Installation
         {
             Ensure = "Present"
             Name = "Microsoft Web Platform Installer 5.0"
             Path = "C:\temp\wpilauncher.exe"
             ProductId = '4D84C195-86F0-4B34-8FDE-4A17EB41306A'
             Arguments = ''
             DependsOn = @("[Script]DownloadWebPIImage")
         }

         Package WebDeploy_Installation
         {
             Ensure = "Present"
             Name = "Microsoft Web Deploy 3.5"
             Path = "$env:ProgramFiles\Microsoft\Web Platform Installer\WebPiCmd-x64.exe"
             ProductId = ''
             Arguments = "/install /products:ASPNET45,ASPNET_REGIIS_NET4,WDeploy  /AcceptEula"
             DependsOn = @("[Package]WebPi_Installation")
         }

         Script DeployWebPackage
         {
             DependsOn = @("[Package]WebDeploy_Installation")
             GetScript = {
                 @{
                     Result = ""
                 }
             }

             TestScript = {
                 $false
             }

             SetScript = {
                 $MSDeployPath = "C:\temp\Deploy"
                 $cmd = "cmd.exe /C `"CD /D {0} && {1} /Y `"  2> C:\temp\Deploy\err.log" -f $MSDeployPath, "contoso.web.deploy.cmd"
                 write-verbose $cmd
                 Invoke-Expression $cmd | write-verbose
             }
         }
     }
 }

 Main
 
# uncomment to test from PowerShell command line
# Main
# Start-DscConfiguration -Path .\Main -Verbose -Wait -Force
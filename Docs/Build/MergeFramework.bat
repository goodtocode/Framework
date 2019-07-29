REM ***********************
REM *** Lib file merge ****
REM ***********************
REM To Get ILMerge: PM> Install-Package ilmerge 
REM PostBuildEvent: Call $(ProjectDir)PostBuildMerge.bat
REM Common are: $(TargetPath) = output file, $(TargetDir) = current full bin path, $(OutDir) = current bin path 
REM Notes: /target:exe /targetplatform:v4,C:\Windows\Microsoft.NET\Framework64\v4.0.30319 /wildcards
REM echo %errorlevel%
REM if errorlevel 1 Then echo failed with %errorlevel%

REM ***
REM *** Variables
REM ***
SET BuildLocationBin=..\bin
SET BuildLocationLib=..\lib
SET BuildLocationDocs=..\docs
SET ILMerge=C:\Projects\Build\Tools\ILMerge\ILMerge.exe

REM ***
REM *** Source DLLs
REM ***
REM *** Extensions ***
SET ExtensionsUniversal=%BuildLocationBin%\Genesys.Framework.Extensions.Core.dll
SET ExtensionsFull=%BuildLocationBin%\Genesys.Framework.Extensions.Standard.dll
SET ExtrasUniversal=%BuildLocationBin%\Genesys.Framework.Extras.Core.dll
SET ExtrasFull=%BuildLocationBin%\Genesys.Framework.Extras.Standard.dll
SET ExtensionsUniversalLib=%BuildLocationLib%\Genesys.Framework.Extensions.Core.dll
SET ExtensionFullsLib=%BuildLocationLib%\Genesys.Framework.Extensions.Standard.dll

REM *** Infrastructure ***
SET Interfaces=%BuildLocationBin%\Genesys.Infrastructure.Interfaces.dll
SET Interop=%BuildLocationBin%\Genesys.Infrastructure.Interoperability.dll
SET DataAccess=%BuildLocationBin%\Genesys.Infrastructure.DataAccess.dll
SET Workflows=%BuildLocationBin%\Genesys.Infrastructure.Workflows.dll
SET MidServices=%BuildLocationBin%\Genesys.Infrastructure.MidServices.dll
SET Models=%BuildLocationBin%\Genesys.Infrastructure.Models.dll
SET AppServices=%BuildLocationBin%\Genesys.Infrastructure.AppServices.dll
SET ResUniversal=%BuildLocationBin%\Genesys.Infrastructure.Resources.Core.dll
SET ResMvc=%BuildLocationBin%\Genesys.Infrastructure.Resources.dll

REM ***
REM *** Destination DLLs
REM ***
REM *** Runtimes ***
SET AppUniversal=%BuildLocationLib%\Genesys.Framework.Application.Core.dll
SET AppMvc=%BuildLocationLib%\Genesys.Framework.Application.Mvc.dll
SET AppWebServices=%BuildLocationLib%\Genesys.Framework.AppServices.dll
SET MidWebServices=%BuildLocationLib%\Genesys.Framework.MidServices.dll
REM *** Docs ***
SET DocsUniversalApp=%BuildLocationDocs%\Genesys.Framework.Application.Core.dll
SET DocsFullApp=%BuildLocationDocs%\Genesys.Framework.Application.Mvc.dll
SET DocsAppServices=%BuildLocationDocs%\Genesys.Framework.AppServices.dll
SET DocsMidServices=%BuildLocationDocs%\Genesys.Framework.MidServices.dll

REM ***
ECHO *** Create and init build location
REM ***
MD %BuildLocationBin%
MD %BuildLocationLib%
MD %BuildLocationDocs%
%WINDIR%\system32\attrib.exe %BuildLocationBin%\*.* -r /s
%WINDIR%\system32\attrib.exe %BuildLocationLib%\*.* -r /s
%WINDIR%\system32\attrib.exe %BuildLocationDocs%\*.* -r /s
Del %BuildLocationLib%\*.log
Del %BuildLocationDocs%\*.log

REM *** 
ECHO *** Merge Runtimes
REM *** 
Del %AppUniversal%
Del %AppMvc%
Del %AppWebServices%
Del %MidWebServices%
Echo *** AppUniversal: %AppUniversal%
%ILMerge% /target:DLL /union /log:%AppUniversal%.log /out:%AppUniversal% %Models% %Interop% %Interfaces% %ExtrasUniversal% %ExtensionsUniversal%
Echo *** AppWebServices: %AppWebServices%
%ILMerge% /target:DLL /union /log:%AppWebServices%.log /out:%AppWebServices% %AppServices% %Models% %Interop% %Interfaces% %ExtrasFull% %ExtensionsFull% %ExtrasUniversal% %ExtensionsUniversal%
REM Echo *** MidWebServices: %MidWebServices%
%ILMerge% /target:DLL /union /log:%MidWebServices%.log /out:%MidWebServices% %MidServices% %Models% %Interop% %Interfaces% %ExtrasFull% %ExtensionsFull% %ExtrasUniversal% %ExtensionsUniversal%
Echo *** AppMvc: %AppMvc%
%ILMerge% /target:DLL /union /log:%AppMvc%.log /out:%AppMvc% %Models% %Interop% %Interfaces% %ExtrasFull% %ExtensionsFull% %ExtrasUniversal% %ExtensionsUniversal%


REM ***
ECHO *** Merge Docs
REM ***
Del %DocsUniversalApp%
Del %DocsFullApp%
Del %DocsAppServices%
Del %DocsMidServices%
Echo *** DocsUniversalApp: %DocsUniversalApp%
%ILMerge% /target:DLL /allowDup /xmldocs /log:%DocsUniversalApp%.log /out:%DocsUniversalApp% %ExtensionsUniversal% %ExtrasUniversal% %Interfaces% %Interop% %Models%
Echo *** DocsFullApp: %DocsFullApp%
%ILMerge% /target:DLL /allowDup /xmldocs /log:%DocsFullApp%.log /out:%DocsFullApp% %ExtensionsUniversal% %ExtrasUniversal% %ExtensionsFull% %ExtrasFull% %Interfaces% %Interop% %Models% %ResMvc%
Echo *** DocsAppServices: %DocsAppServices%
%ILMerge% /target:DLL /allowDup /xmldocs /log:%DocsAppServices%.log /out:%DocsAppServices% %ExtensionsUniversal% %ExtrasUniversal% %ExtensionsFull% %ExtrasFull% %Interfaces% %Interop% %Models% %AppServices%
Echo *** DocsMidServices: %DocsMidServices%
%ILMerge% /target:DLL /allowDup /xmldocs /log:%DocsMidServices%.log /out:%DocsMidServices% %ExtensionsUniversal% %ExtrasUniversal% %ExtensionsFull% %ExtrasFull% %Interfaces% %Interop% %DataAccess% %Workflows% %MidServices% 

Echo *** Merge Complete ***
REM exit 0
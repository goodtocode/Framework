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
SET ExtensionsUniversal=%BuildLocationBin%\GoodToCode.Framework.Extensions.Core.dll
SET ExtensionsFull=%BuildLocationBin%\GoodToCode.Framework.Extensions.Standard.dll
SET ExtrasUniversal=%BuildLocationBin%\GoodToCode.Framework.Extras.Core.dll
SET ExtrasFull=%BuildLocationBin%\GoodToCode.Framework.Extras.Standard.dll
SET ExtensionsUniversalLib=%BuildLocationLib%\GoodToCode.Framework.Extensions.Core.dll
SET ExtensionFullsLib=%BuildLocationLib%\GoodToCode.Framework.Extensions.Standard.dll

REM *** Infrastructure ***
SET Interfaces=%BuildLocationBin%\GoodToCode.Infrastructure.Interfaces.dll
SET Interop=%BuildLocationBin%\GoodToCode.Infrastructure.Interoperability.dll
SET DataAccess=%BuildLocationBin%\GoodToCode.Infrastructure.DataAccess.dll
SET Workflows=%BuildLocationBin%\GoodToCode.Infrastructure.Workflows.dll
SET MidServices=%BuildLocationBin%\GoodToCode.Infrastructure.MidServices.dll
SET Models=%BuildLocationBin%\GoodToCode.Infrastructure.Models.dll
SET AppServices=%BuildLocationBin%\GoodToCode.Infrastructure.AppServices.dll
SET ResUniversal=%BuildLocationBin%\GoodToCode.Infrastructure.Resources.Core.dll
SET ResMvc=%BuildLocationBin%\GoodToCode.Infrastructure.Resources.dll

REM ***
REM *** Destination DLLs
REM ***
REM *** Runtimes ***
SET AppUniversal=%BuildLocationLib%\GoodToCode.Framework.Application.Core.dll
SET AppMvc=%BuildLocationLib%\GoodToCode.Framework.Application.Mvc.dll
SET AppWebServices=%BuildLocationLib%\GoodToCode.Framework.AppServices.dll
SET MidWebServices=%BuildLocationLib%\GoodToCode.Framework.MidServices.dll
REM *** Docs ***
SET DocsUniversalApp=%BuildLocationDocs%\GoodToCode.Framework.Application.Core.dll
SET DocsFullApp=%BuildLocationDocs%\GoodToCode.Framework.Application.Mvc.dll
SET DocsAppServices=%BuildLocationDocs%\GoodToCode.Framework.AppServices.dll
SET DocsMidServices=%BuildLocationDocs%\GoodToCode.Framework.MidServices.dll

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
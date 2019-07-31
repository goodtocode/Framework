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
SET ExtensionsUniversal=%BuildLocationBin%\GoodToCode.Framework.Extensions.Core.dll
SET ExtensionsFull=%BuildLocationBin%\GoodToCode.Framework.Extensions.Standard.dll
SET ExtrasUniversal=%BuildLocationBin%\GoodToCode.Framework.Extras.Core.dll
SET ExtrasFull=%BuildLocationBin%\GoodToCode.Framework.Extras.Standard.dll

REM ***
REM *** Destination DLLs
REM ***
REM *** Runtimes
SET DestinationUniversal=%BuildLocationLib%\GoodToCode.Framework.Extensions.Core.dll
SET DestinationFull=%BuildLocationLib%\GoodToCode.Framework.Extensions.Standard.dll
REM *** Docs ***
SET DocsUniversal=%BuildLocationDocs%\GoodToCode.Framework.Extensions.Core.dll
SET DocsFull=%BuildLocationDocs%\GoodToCode.Framework.Extensions.Standard.dll

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
cd %BuildLocationBin%
Del %DestinationUniversal%
Del %DestinationFull%
%ILMerge% /target:DLL /union /log:%DestinationUniversal%.log /out:%DestinationUniversal% %ExtensionsUniversal% %ExtrasUniversal%
Echo Done - %DestinationUniversal%
%ILMerge% /target:DLL /union /log:%DestinationFull%.log /out:%DestinationFull% %ExtensionsUniversal% %ExtrasUniversal% %ExtensionsFull% %ExtrasFull%
Echo Done - %DestinationFull%

REM ***
ECHO *** Merge Docs
REM ***
CD %BuildLocationDocs%
Del %DocsUniversal%
Del %DocsFull%
%ILMerge% /target:DLL /allowDup /xmldocs /log:%DocsUniversal%.log /out:%DocsUniversal% %ExtensionsUniversal% %ExtrasUniversal%
Echo Done - %DocsUniversal%
%ILMerge% /target:DLL /allowDup /xmldocs /log:%DocsFull%.log /out:%DocsFull% %ExtensionsUniversal% %ExtrasUniversal% %ExtensionsFull% %ExtrasFull%
Echo Done - %DocsFull%

Echo *** Merge Complete ***
REM exit 0
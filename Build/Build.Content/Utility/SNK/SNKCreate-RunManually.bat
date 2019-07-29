ECHO OFF
REM Usage: Call SNKCreate.bat GenesysFramework.snk
ECHO Starting SNKCreate.bat GenesysFramework.snk

%WINDIR%\system32\attrib.exe *.* -r /s

..\Utility\SNK\sn.exe –k GenesysFramework.snk

exit 0

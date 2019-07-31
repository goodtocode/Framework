ECHO OFF
REM Usage: Call SNKCreate.bat GoodToCode.snk
ECHO Starting SNKCreate.bat GoodToCode.snk

%WINDIR%\system32\attrib.exe *.* -r /s

..\Utility\SNK\sn.exe –k GoodToCode.snk

exit 0

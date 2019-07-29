@echo off set VS_PATH=Microsoft\VisualStudio 

cd %LOCALAPPDATA%\%VS_PATH% echo cd:%cd%
for /f %%G in ('dir /b "*Exp"') do ( echo Found %%G rmdir /S /Q %%G )
cd %APPDATA%\%VS_PATH% echo cd:%cd%
for /f %%G in ('dir /b "*Exp"') do ( echo Found %%G rmdir /S /Q %%G )
pause 
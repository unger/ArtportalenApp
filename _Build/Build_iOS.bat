@echo off

setlocal enabledelayedexpansion

set platform=iOS
for /f "delims=" %%i in ('..\_tools\VersionBumper.exe ios "C:\Projects\GitHub\ArtportalenApp\ArtportalenApp.iOS\Info.plist"') do set BuildVersion=%%i

call _include.cmd


%MSBuildCustomPath% ..\ArtportalenApp.iOS\ArtportalenApp.iOS.csproj /fl /flp:logfile="%MsBuildLogFile%" /t:Clean,Build /p:Configuration=Ad-Hoc;Platform=iPhone;ServerAddress=10.112.0.125;ServerUser=Layer10;OutputPath=%tempFolderAbsolute%


find "was built while disconnected from a Mac agent, so only the main assembly was compiled" "%MsBuildLogFile%" >NUL
if %ERRORLEVEL% == 0 (
	Call:echored "Build server control connection failed, please see the file %MsBuildLogFile% for more info"
	pause
	exit 1 /b
) else (
	Call:echogreen "Build succeeded"
	SET ERRORLEVEL=0
)

copy %tempFolder%\*.ipa "%outputFolder%" >NUL

rem rmdir %tempFolder% /q /s 


if %ERRORLEVEL% == 0 (
	Call:echogreen "Great success!"
) else (
	Call:echored "Something went wrong"
)



pause

goto:eof


:ECHORED
%Windir%\System32\WindowsPowerShell\v1.0\Powershell.exe write-host -foregroundcolor Red %1
goto:eof

:ECHOGREEN
%Windir%\System32\WindowsPowerShell\v1.0\Powershell.exe write-host -foregroundcolor Green %1
goto:eof
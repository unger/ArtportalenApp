@echo off

setlocal enabledelayedexpansion

set platform=Android
for /f "delims=" %%i in ('..\_tools\VersionBumper.exe android "C:\Projects\GitHub\ArtportalenApp\ArtportalenApp.Droid\Properties\AndroidManifest.xml"') do set BuildVersion=%%i

call _include.cmd


%MSBuildCustomPath% ..\ArtportalenApp.Droid\ArtportalenApp.Droid.csproj /fl /flp:logfile="%MsBuildLogFile%" /t:Clean,Build,SignAndroidPackage /p:Configuration=Release;Platform=AnyCPU;OutputPath=%tempFolderAbsolute%

copy %tempFolder%\*.apk "%outputFolder%" >NUL

rmdir %tempFolder% /q /s 

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



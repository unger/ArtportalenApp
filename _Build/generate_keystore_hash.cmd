@echo off

set keytool="C:\Program Files (x86)\Java\jdk1.7.0_71\bin\keytool"
set openssl=..\_tools\openssl

set keytoolDebugCommand=%keytool% -exportcert -alias androiddebugkey -keystore "%LocalAppData%\Xamarin\Mono for Android\debug.keystore" -storepass android 
Call:echogreen "Debug keystore"
%keytoolDebugCommand%
echo.
Call:echogreen "Debug keystore Hash"
%keytoolDebugCommand% | %openssl% sha1 -binary | %openssl% base64
echo.
echo.


%keytool% -list -v -keystore "%LocalAppData%\Xamarin\Mono for Android\debug.keystore" -alias androiddebugkey -storepass android -keypass android

pause



goto:eof


:ECHORED
%Windir%\System32\WindowsPowerShell\v1.0\Powershell.exe write-host -foregroundcolor Red %1
goto:eof

:ECHOGREEN
%Windir%\System32\WindowsPowerShell\v1.0\Powershell.exe write-host -foregroundcolor Green %1
goto:eof
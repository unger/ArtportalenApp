::@echo off


for /f "delims=" %%i in ('..\_tools\VersionBumper.exe android "C:\Projects\GitHub\ArtportalenApp\ArtportalenApp\ArtportalenApp.Droid\Properties\AndroidManifest.xml"') do set VERSION=%%i
Echo AndroidVersion: %VERSION%


for /f "delims=" %%i in ('..\_tools\VersionBumper.exe ios "C:\Projects\GitHub\ArtportalenApp\ArtportalenApp\ArtportalenApp.iOS\Info.plist"') do set VERSION=%%i
Echo iOSVersion: %VERSION%


pause

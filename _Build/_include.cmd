@echo off

set DateTime=
Call:GetDateTime DateTime

chcp 437 >NUL

set outputFolder=output\%platform%_%BuildVersion%
set outputFolderAbsolute=%cd%\%outputFolder%
set tempFolder=output\%platform%_temp
set tempFolderAbsolute=%cd%\%tempFolder%\

mkdir "%outputFolder%" >NUL


set MsBuildLogFile=%outputFolder%\msbuild_%platform%_%DateTime%.log

if not defined MSBuildCustomPath (
	set MSBuildCustomPath="%ProgramFiles(x86)%\MSBuild\12.0\Bin\MSBuild.exe"
)



goto:eof


:GetDateTime       -- function description here
::                 -- %~1: return into variable
SETLOCAL
REM.--function body here

for /F "usebackq tokens=1,2 delims==" %%i in (`wmic os get LocalDateTime /VALUE 2^>NUL`) do if '.%%i.'=='.LocalDateTime.' set ldt=%%j
set ldt=%ldt:~0,4%%ldt:~4,2%%ldt:~6,2%_%ldt:~8,2%%ldt:~10,2%%ldt:~12,2%

(ENDLOCAL & REM -- RETURN VALUES
    IF "%~1" NEQ "" SET %~1=%ldt%
)
goto:eof


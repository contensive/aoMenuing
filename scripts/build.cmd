
rem @echo off

rem 
rem Must be run from the projects git\project\scripts folder - everything is relative
rem run >build [deploymentNumber]
rem deploymentNumber is YYMMDD.build-number, like 190824.5
rem
rem Setup deployment folder
rem

rem -- Release or Debug
set DebugRelease=Debug

set collectionName=aoMenuing
set solutionName=aoMenuing.sln
set collectionPath=..\collections\aoMenuing\
set binPath=..\server\aoMenuing\bin\%DebugRelease%\
set msbuildLocation=C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\
set deploymentFolderRoot=C:\deployments\aoMenuing\Dev\

set deploymentNumber=%1
set year=%date:~12,4%
set month=%date:~4,2%
set day=%date:~7,2%

rem
rem if deployment number not entered, set it to date.1
rem
IF [%deploymentNumber%] == [] (
	echo No deployment folder provided on the command line, use current date
	set deploymentTimeStamp=%year%%month%%day%
)
rem
rem if deployment folder exists, delete it and make directory
rem

set suffix=1
:tryagain
set deploymentNumber=%deploymentTimeStamp%.%suffix%
if not exist "%deploymentFolderRoot%%deploymentNumber%" goto :makefolder
set /a suffix=%suffix%+1
goto tryagain
:makefolder
md "%deploymentFolderRoot%%deploymentNumber%"

rem ==============================================================
rem
rem copy UI files
rem

copy ..\UI\NavbarNavULDefaultLayout.html ..\collections\aoMenuing


pause

rem ==============================================================
rem
echo build 
rem
cd ..\server
"%msbuildLocation%msbuild.exe" %solutionName% /property:Configuration=%DebugRelease%
if errorlevel 1 (
   echo failure building
   pause
   exit /b %errorlevel%
)
cd ..\scripts

rem ==============================================================
rem
echo Build addon collection
rem

rem copy bin folder assemblies to collection folder
copy "%binPath%*.dll" "%collectionPath%"

rem create new collection zip file
c:
cd %collectionPath%
del "%collectionName%.zip" /Q
"c:\program files\7-zip\7z.exe" a "%collectionName%.zip"
xcopy "%collectionName%.zip" "%deploymentFolderRoot%%deploymentNumber%" /Y
cd ..\..\scripts

rem ==============================================================
rem
rem delete UI file copies
rem

del ..\collections\aoMenuing\NavbarNavULDefaultLayout.html
del ..\collections\aoMenuing\*.dll

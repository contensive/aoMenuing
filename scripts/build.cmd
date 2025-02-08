
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
set msbuildLocation=C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\
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

del ..\collections\HelpPages\uiHelpPages.zip

rem pause

rem ==============================================================
rem
rem create helpfiles.zip file for install in private/helpfiles/
rem 
rem make a \help folder in the addon Git folder and store the collections markup files there. 
rem a period in the filename represents a topic on the navigation, so to make an article "Shopping" in the "Ecommerce" topic, create a document "Ecommerce.Shopping.md"
rem help files are installed in the "privateFiles\helpfiles\(collectionname)" folder. The collectionname must match the addoon collections name exactly.
rem add a resource node to the collection xml file to install the helpfile zip to the site. For example
rem    <Resource name="HelpFiles.zip" type="privatefiles" path="helpfiles/(collectionname)" />
rem then if the first install, 
rem

cd ..\help
del %collectionPath%HelpFiles.zip

rem copy default article and articles for the  Help Pages collection
"c:\program files\7-zip\7z.exe" a "%collectionPath%HelpFiles.zip" 
cd ..\scripts

rem ==============================================================
rem
rem copy UI files
rem

copy ..\UI\NavbarNavULDefaultLayout.html ..\collections\aoMenuing

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

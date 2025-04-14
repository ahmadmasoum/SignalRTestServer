@echo off
echo SignalR Test Server (HTTPS Version)
echo Current Date and Time (UTC): 2025-04-14 08:36:59
echo Current User: ahmadmasoumi

set PUBLISH_FOLDER=publish

echo.
echo Setting up HTTPS development certificate...
echo This will create and trust a local development certificate for HTTPS.
echo.

REM Try to create and trust a development certificate
dotnet dev-certs https --clean
dotnet dev-certs https --trust
if %ERRORLEVEL% neq 0 (
    echo.
    echo Warning: Certificate setup may have failed. 
    echo This might cause connection issues with browsers.
    echo You may need to manually trust the certificate.
    echo.
    pause
)

REM Check if publish folder exists
if not exist %PUBLISH_FOLDER%\SignalRTestServer.dll (
    echo Publish folder not found or incomplete. Publishing project...
    dotnet publish -c Release -o %PUBLISH_FOLDER%
    if %ERRORLEVEL% neq 0 (
        echo Error publishing project. Exiting.
        exit /b %ERRORLEVEL%
    )
    echo Project published successfully.
) else (
    echo Using existing publish folder.
)

echo.
echo Starting SignalR Test Server from publish folder...
echo.

REM Run the published application
cd %PUBLISH_FOLDER%
SignalRTestServer.exe

REM If executable fails, try using dotnet command
if %ERRORLEVEL% neq 0 (
    echo Direct execution failed, trying with dotnet command...
    dotnet SignalRTestServer.dll
)

cd ..
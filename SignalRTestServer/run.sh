#!/bin/bash
echo "Setting up HTTPS development certificate..."
dotnet dev-certs https --clean
dotnet dev-certs https --trust

echo "Building SignalR Test Server..."
dotnet build -c Release
echo ""
echo "Starting SignalR Test Server..."
echo ""
dotnet run -c Release --no-build
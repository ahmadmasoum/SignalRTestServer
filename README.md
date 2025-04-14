# SignalR Test Server

A lightweight testing server for SignalR connections to help mobile developers test their client implementations.

## Features

- Simple SignalR hub with echo and message broadcasting
- Supports both HTTP (port 5000) and HTTPS (port 5001)
- Browser-based test client with troubleshooting tools
- Command-line interface for server control

## Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later

### Running the Server

1. Run the server using the provided script:
   - On Windows: `run.bat`
   - On Linux/macOS: `chmod +x run.sh && ./run.sh`

2. The server will start and listen on:
   - HTTP: http://0.0.0.0:5000/testhub
   - HTTPS: https://0.0.0.0:5001/testhub

### Using the Browser Client

1. Open `signalr-test.html` in your browser
2. Enter the hub URL (e.g., `https://localhost:5001/testhub`)
3. Click "Test Connection" then "Connect"
4. Start sending messages

### Chrome Certificate Issues

When using Chrome with self-signed certificates:

1. Open `https://localhost:5001/healthcheck` in Chrome
2. When you see "Your connection is not private" error
3. Type `thisisunsafe` directly on the keyboard
4. Chrome will remember this certificate bypass

## Server Commands

- `broadcast <message>` - Send a message to all clients
- `clients` - List all connected clients
- `exit` - Stop the server

## For Mobile Developers

See the MOBILE_GUIDE.md file for details on connecting from Flutter, Android, and iOS.

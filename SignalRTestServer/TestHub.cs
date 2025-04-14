using Microsoft.AspNetCore.SignalR;

namespace SignalRTestServer;

public class TestHub : Hub
{
    public static int ConnectedClients { get; private set; }
    public static HashSet<string> ConnectedIds { get; } = new();

    public override async Task OnConnectedAsync()
    {
        ConnectedClients++;
        ConnectedIds.Add(Context.ConnectionId);

        string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        Console.WriteLine($"[{timestamp}] Client connected: {Context.ConnectionId}");
        Console.WriteLine($"[{timestamp}] Total clients: {ConnectedClients}");

        // Send welcome message to the connecting client
        await Clients.Caller.SendAsync("ReceiveMessage", "Server", $"Welcome to the test server! Connected at {timestamp}");

        // Notify all clients about new connection
        await Clients.Others.SendAsync("ReceiveMessage", "Server", "New client connected");

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        ConnectedClients--;
        ConnectedIds.Remove(Context.ConnectionId);

        string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        Console.WriteLine($"[{timestamp}] Client disconnected: {Context.ConnectionId}");
        Console.WriteLine($"[{timestamp}] Total clients: {ConnectedClients}");

        // Notify all clients about disconnection
        await Clients.Others.SendAsync("ReceiveMessage", "Server", "A client disconnected");

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string user, string message)
    {
        string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        Console.WriteLine($"[{timestamp}] Message from {user}: {message}");
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task Echo(string message)
    {
        string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        Console.WriteLine($"[{timestamp}] Echo request: {message}");
        await Clients.Caller.SendAsync("EchoResponse", message);
    }
}
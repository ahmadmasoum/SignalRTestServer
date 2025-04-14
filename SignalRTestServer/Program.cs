using Microsoft.AspNetCore.SignalR;
using SignalRTestServer;

// Banner
string timestamp = DateTime.UtcNow.ToString("u");
string user = "ahmadmasoum";
Console.WriteLine($"Starting SignalR Test Server (.NET 9)...");
Console.WriteLine($"Current Date and Time (UTC): {timestamp}");
Console.WriteLine($"Current User: {user}");

// Create and configure the web application
var builder = WebApplication.CreateBuilder(args);

// Add services to container
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
        builder.SetIsOriginAllowed(_ => true)
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials());
});

// Configure HTTPS and HTTP
builder.WebHost.UseUrls("https://0.0.0.0:5001", "http://0.0.0.0:5000");

var app = builder.Build();

// Configure middleware pipeline
app.UseCors("CorsPolicy");

// Add health check endpoint for certificate testing
app.MapGet("/healthcheck", () => $"SignalR Test Server is running. Current time: {DateTime.UtcNow}");

app.MapHub<TestHub>("/testhub");

// Start server
Console.WriteLine($"SignalR Server running at:");
Console.WriteLine($"  - HTTP:  http://0.0.0.0:5000/testhub");
Console.WriteLine($"  - HTTPS: https://0.0.0.0:5001/testhub");
Console.WriteLine($"Health check available at /healthcheck endpoint");
Console.WriteLine("Available commands:");
Console.WriteLine("  broadcast <message> - Send message to all connected clients");
Console.WriteLine("  clients - List connected clients");
Console.WriteLine("  exit - Stop the server and exit");

var hubContext = app.Services.GetRequiredService<IHubContext<TestHub>>();

// Start the web server in a background task
var serverTask = app.RunAsync();

// Command loop
while (true)
{
    var input = Console.ReadLine();
    if (string.IsNullOrEmpty(input))
        continue;

    timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

    if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }
    else if (input.StartsWith("broadcast ", StringComparison.OrdinalIgnoreCase))
    {
        var message = input["broadcast ".Length..];
        await hubContext.Clients.All.SendAsync("ReceiveMessage", "Server", message);
        Console.WriteLine($"[{timestamp}] Broadcast sent: {message}");
    }
    else if (input.Equals("clients", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine($"[{timestamp}] Connected clients: {TestHub.ConnectedClients}");
        foreach (var clientId in TestHub.ConnectedIds)
        {
            Console.WriteLine($"  - {clientId}");
        }
    }
    else
    {
        Console.WriteLine($"[{timestamp}] Unknown command");
    }
}

// Stop the application
await app.StopAsync();
Console.WriteLine($"[{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")}] Server stopped");

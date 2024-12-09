using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Test_skola_app;
using Test_skola_app.Data;
using Test_skola_app.Models;

public class ApiService
{
    private readonly DatabaseService _databaseService;

    public ApiService(string dbPath)
    {
        _databaseService = new DatabaseService(dbPath);
    }

    public async Task StartAsync()
    {
        var httpListener = new HttpListener();
        httpListener.Prefixes.Add("http://localhost:8080/");
        httpListener.Start();

        Console.WriteLine("WebSocket API Server is running at ws://localhost:8080/");

        while (true)
        {
            var context = await httpListener.GetContextAsync();

            if (context.Request.IsWebSocketRequest)
            {
                var webSocketContext = await context.AcceptWebSocketAsync(null);
                await HandleConnectionAsync(webSocketContext.WebSocket);
            }
        }
    }

    private async Task HandleConnectionAsync(WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];

        while (webSocket.State == WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Close)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                break;
            }

            var receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
            Console.WriteLine($"Received: {receivedMessage}");

            var response = await ProcessRequestAsync(receivedMessage);
            await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(response)), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }

    private async Task<string> ProcessRequestAsync(string request)
    {
        try
        {
            var command = JsonSerializer.Deserialize<ApiCommand>(request);

            return command.Operation.ToLower() switch
            {
                "create_user" => await CreateUserAsync(command.Payload),
                "read_users" => await ReadUsersAsync(),
                "delete_user" => await DeleteUserAsync(command.Payload),
                _ => JsonSerializer.Serialize(new { Error = "Unknown command" })
            };
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { Error = ex.Message });
        }
    }

    private async Task<string> CreateUserAsync(JsonElement payload)
    {
        
        var user = JsonSerializer.Deserialize<User>(payload.GetRawText());
        await _databaseService.SaveUserAsync(user);
        return JsonSerializer.Serialize(new { Success = true, Message = "User created." });
    }

    private async Task<string> ReadUsersAsync()
    {
        var users = await _databaseService.GetUsersAsync();
        return JsonSerializer.Serialize(users);
    }

    private async Task<string> DeleteUserAsync(JsonElement payload)
    {
        var id = payload.GetProperty("Id").GetInt32();
        var users = await _databaseService.GetUsersAsync();
        var user = users.Find(u => u.Id == id);

        if (user == null)
            return JsonSerializer.Serialize(new { Error = "User not found." });

        await _databaseService.DeleteUserAsync(user);
        return JsonSerializer.Serialize(new { Success = true, Message = "User deleted." });
    }
}

public class ApiCommand
{
    public string Operation { get; set; }
    public JsonElement Payload { get; set; }
}
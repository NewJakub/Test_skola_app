using System.Net.WebSockets;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "asteroids.db");
        var apiService = new ApiService(dbPath);
        await apiService.StartAsync();
        using var client = new ClientWebSocket();
        try
        {
            // Connect to the WebSocket server
            await client.ConnectAsync(new Uri("ws://localhost:8080/"), CancellationToken.None);
            Console.WriteLine("Connected to WebSocket server.");

            // Send a create request
            var createUserRequest = new
            {
                Operation = "create_user",
                Payload = new
                {
                    Name = "John Doe",
                    Email = "johndoe@example.com"
                }
            };
            await SendRequestAsync(client, createUserRequest);

            // Send a read request
            var readUsersRequest = new
            {
                Operation = "read_users",
                Payload = new { }
            };
            await SendRequestAsync(client, readUsersRequest);

            // Close the connection
            await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client done", CancellationToken.None);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    private static async Task SendRequestAsync(ClientWebSocket client, object request)
    {
        var jsonRequest = JsonSerializer.Serialize(request);
        var buffer = Encoding.UTF8.GetBytes(jsonRequest);

        // Send the request
        await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        Console.WriteLine($"Sent: {jsonRequest}");

        // Receive the response
        var responseBuffer = new byte[1024 * 4];
        var result = await client.ReceiveAsync(new ArraySegment<byte>(responseBuffer), CancellationToken.None);

        var responseJson = Encoding.UTF8.GetString(responseBuffer, 0, result.Count);
        Console.WriteLine($"Received: {responseJson}");
    }
}

using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

public class GeneralModule : ModuleBase<SocketCommandContext>
{
    // The library will automatically inject the client here 
    // because it's registered in the ServiceProvider
    private readonly DiscordSocketClient _client;

    public GeneralModule(DiscordSocketClient client)
    {
        _client = client;
    }

    [Command("ping")]
    public async Task PingAsync()
    {
        // You can use _client here, or Context.Client
        await ReplyAsync($"Pong! Latency is {_client.Latency}ms");
    }
}
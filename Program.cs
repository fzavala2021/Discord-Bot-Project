// See https://aka.ms/new-console-template for more information
using Discord;
using Discord.WebSocket;


DiscordSocketClient _client = new DiscordSocketClient();
_client.Log += Log;
// Example startup (don't hardcode bot tokens in source)
var tokenPath = "Config\\nachoToken.json";
//var tokenPath = "Config\\vezToken.json";
var tokenGetter = new ParseToken(tokenPath);
var token = tokenGetter.GetToken();
await _client.LoginAsync(TokenType.Bot, token);
await _client.StartAsync();

// keep the app running
await Task.Delay(-1);

static Task Log(LogMessage msg)
{
    Console.WriteLine(msg.ToString());
    return Task.CompletedTask;
}



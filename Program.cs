// See https://aka.ms/new-console-template for more information
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;


// Configure DI
var services = new ServiceCollection()
    .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig { MessageCacheSize = 100, GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent }))
    .AddSingleton(new CommandService(new CommandServiceConfig { CaseSensitiveCommands = false }))
    .BuildServiceProvider();

var client = services.GetRequiredService<DiscordSocketClient>();
client.Log += Log;
var commands = services.GetRequiredService<CommandService>();

// Token from helper
var tokenPath = "Config\\nachoToken.json";
ParseToken tokenGetter = new ParseToken(tokenPath);
var token = tokenGetter.GetToken();
if (string.IsNullOrWhiteSpace(token))
{
    Console.WriteLine("Bot token missing.");
    return;
}

await commands.AddModulesAsync(System.Reflection.Assembly.GetEntryAssembly(), services);

client.MessageReceived += async rawMessage =>
{
    if (rawMessage is not SocketUserMessage message || message.Author.IsBot)
        return;

    int argPos = 0;
    if (!message.HasCharPrefix('!', ref argPos))
        return;

    var context = new SocketCommandContext(client, message);
    var result = await commands.ExecuteAsync(context, argPos, services);
    if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
        await message.Channel.SendMessageAsync(result.ErrorReason);
};

await client.LoginAsync(TokenType.Bot, token);
await client.StartAsync();

// keep the app running
await Task.Delay(-1);

static Task Log(LogMessage msg)
{
    Console.WriteLine(msg.ToString());
    return Task.CompletedTask;
}
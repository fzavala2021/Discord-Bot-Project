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

CommandHandler commandHandler = new CommandHandler(client, commands, services);
await commandHandler.InitializeAsync();

await client.LoginAsync(TokenType.Bot, token);
await client.StartAsync();

// keep the app running
await Task.Delay(-1);

static Task Log(LogMessage msg)
{
    Console.WriteLine(msg.ToString());
    return Task.CompletedTask;
}
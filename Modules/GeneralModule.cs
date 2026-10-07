using Discord.Commands;
using Discord.WebSocket;

public class GeneralModule : ModuleBase<SocketCommandContext>
{
    [Command("ping")]
    [Summary("Returns bot latency.")]
    public async Task PingAsync()
    {
        // Use Context.Client so the module works without DI
        var client = Context.Client as DiscordSocketClient;
        var latency = client?.Latency ?? 0;
        await ReplyAsync($"Pong! Latency is {latency}ms");
    }

    [Command("echo")]
    [Summary("Echoes the provided text.")]
    public async Task EchoAsync([Remainder][Summary("Text to echo")] string? text = null)
    {
        if (string.IsNullOrWhiteSpace(text))
            await ReplyAsync("Usage: !echo <text>");
        else
            await ReplyAsync(text);
    }

    [Command("userinfo")]
    [Alias("user", "whois")]
    [Summary("Returns info about the current or specified user.")]
    public async Task UserInfoAsync(SocketUser? user = null)
    {
        var target = user ?? Context.User;
        await ReplyAsync($"{target.Username}#{target.Discriminator}");
    }

    [Command("roll")]
    [Summary("Rolls a die with the given number of sides (default 6).")]
    public async Task RollAsync(int sides = 6)
    {
        if (sides < 2 || sides > 1000)
        {
            await ReplyAsync("Usage: !roll [sides] (between 2 and 1000)");
            return;
        }

        var result = Random.Shared.Next(1, sides + 1);
        await ReplyAsync($"{Context.User.Mention} rolled a **{result}** (d{sides})");
    }
}
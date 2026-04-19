using System.Text.Json;

public class ParseToken
{
    private readonly string _filePath;

    public ParseToken(string filePath)
    {
        _filePath = filePath;
    }

    public string? GetToken()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine($"File not found: {_filePath}");
                return null;
            }
            using var streamReader = new StreamReader(_filePath);
            var json = streamReader.ReadToEnd();
            var document = JsonDocument.Parse(json);
                if (document.RootElement.TryGetProperty("token", out var tokenElement))
                {
                    return tokenElement.GetString();
                }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error accessing file: {ex.Message}");
            return null;
        }

    }
}
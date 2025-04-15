namespace Base.Config;

public class GeminiOption
{
    public string Endpoint { get; set; } = default!;
    public string ApiKey { get; set; } = default!;
}

public class MessageType
{
    public string Role { get; set; } = default!;
    public List<Part> Parts { get; set; } = new();
}

public class Part
{
    public string text { get; set; } = default!;
}

public class GeminiWrapperResponse
{
    public List<Candidate> candidates { get; set; } = new();
}

public class Candidate
{
    public Content content { get; set; } = new();
}

public class Content
{
    public List<Part> parts { get; set; } = new();
}

public class GeminiResponse
{
    public string response_text { get; set; } = default!;
    public string planUML { get; set; } = default!;
}
using Base.Config;

namespace Base.Gemini.Handler;

public interface IGeminiService
{
    public Task<GeminiResponse> PostAnswerAsync(List<MessageType> messages);
}

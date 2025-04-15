﻿using Base.Config;
using System.Text;
using System.Text.Json;

namespace Base.Gemini.Handler;

public class GeminiService : IGeminiService
{
    private readonly HttpClient _httpClient;
    private readonly GeminiOption _options;

    public GeminiService(HttpClient httpClient, GeminiOption options)
    {
        _httpClient = httpClient;
        _options = options;
    }

    public async Task<GeminiResponse> PostAnswerAsync(List<MessageType> messages)
    {
        var url = $"?key={_options.ApiKey}";

        var requestBody = new
        {
            contents = messages.Select(m => new
            {
                role = m.Role,
                parts = m.Parts.Select(p => new { text = p.text })
            }),
            generationConfig = new
            {
                response_mime_type = "application/json",
                response_schema = new
                {
                    type = "object",
                    properties = new
                    {
                        response_text = new { type = "string", description = "Phản hồi từ AI về tình trạng của bệnh nhân" },
                        planUML = new { type = "string", description = "Đây là phần plant UML code để vẽ diagram theo yêu cầu người dùng" }
                    },
                    required = new[] { "response_text", "planUML" }
                }
            }
        };

        var jsonContent = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync(url, jsonContent);

        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            var wrapper = JsonSerializer.Deserialize<GeminiWrapperResponse>(responseString);

            if (wrapper?.candidates?.Count > 0)
            {
                var jsonText = wrapper.candidates[0].content.parts[0].text ;

                // Lúc này jsonText vẫn là string chứa JSON, nên deserialize lần 2
                var finalResponse = JsonSerializer.Deserialize<GeminiResponse>(jsonText);
                if (finalResponse != null)
                {
                    return finalResponse;
                }
            }
        }

        return null;
    }
}

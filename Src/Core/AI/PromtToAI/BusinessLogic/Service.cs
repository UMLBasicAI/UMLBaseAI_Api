using System.Reflection;
using System.Security.Claims;
using Base.Gemini.Handler;
using FCommon.AccessToken;
using FCommon.Constants;
using FCommon.FeatureService;
using FCommon.RefreshToken;
using Microsoft.AspNetCore.Http;
using PromptToAI.Common;
using PromptToAI.DataAccess;
using PromptToAI.Models;

namespace PromptToAI.BusinessLogic;

public sealed class Service : IServiceHandler<AppRequestModel, AppResponseModel>
{
    private readonly Lazy<IRepository> _repository;
    private readonly Lazy<IGeminiService> _geminiService;
    private readonly Lazy<IHttpContextAccessor> _httpContextAccessor;

    public Service(
        Lazy<IRepository> repository,
        Lazy<IGeminiService> geminiService,
        Lazy<IHttpContextAccessor> httpContextAccessor
    )
    {
        _repository = repository;
        _geminiService = geminiService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AppResponseModel> ExecuteAsync(
    AppRequestModel request,
    CancellationToken cancellationToken
)
    {
        if (string.IsNullOrWhiteSpace(request.Prompt))
        {
            return new AppResponseModel { AppCode = Constant.AppCode.VALIDATION_FAILED };
        }

        var historyId = request.HistoryId;
        var prompt = request.Prompt;

        List<Base.Config.MessageType> messages;

        // Nếu có historyId → lấy 10 messages gần nhất để gửi cho Gemini
        if (!string.IsNullOrWhiteSpace(historyId))
        {
            var historyGuid = Guid.Parse(historyId);
            var history = await _repository.Value.getHistoryById(historyGuid, cancellationToken);
            var recentMessages = await _repository.Value.getNewestMessagesOfHistory(historyGuid, cancellationToken);

            // Build messages để gửi lên Gemini (dựa theo MessageType và Content)
            messages = recentMessages
                .OrderBy(m => m.CreatedAt)
                .TakeLast(10)
                .Select(m => new Base.Config.MessageType
                {
                    Role = m.MessageType == "request" ? "user" : "model",
                    Parts = new List<Base.Config.Part> { new Base.Config.Part { text = m.Content } }
                })
                .ToList();

            // Thêm prompt hiện tại (message mới) vào danh sách
            messages.Add(new Base.Config.MessageType
            {
                Role = "user",
                Parts = new List<Base.Config.Part> { new Base.Config.Part { text = prompt } }
            });

            // Gửi luôn PlantUML của history vào Gemini nếu cần dùng
            if (history != null && !string.IsNullOrEmpty(history.PlantUMLCode))
            {
                messages.Insert(0, new Base.Config.MessageType
                {
                    Role = "user",
                    Parts = new List<Base.Config.Part>
                    {
                        new Base.Config.Part
                        {
                            text = $"Note: This is the previous PlantUML code you generated. Please consider it in the context:\n{history.PlantUMLCode}"
                        }
                    }
                });
            }
        }
        else
        {
            // Nếu chưa có history → tạo mới
            messages = new List<Base.Config.MessageType>
        {
            new Base.Config.MessageType
            {
                Role = "user",
                Parts = new List<Base.Config.Part> { new Base.Config.Part { text = prompt } }
            }
        };
        }

        // Gọi Gemini
        var geminiResponse = await _geminiService.Value.PostAnswerAsync(messages);

        if (
            geminiResponse == null ||
            string.IsNullOrWhiteSpace(geminiResponse.planUML) ||
            string.IsNullOrWhiteSpace(geminiResponse.response_text)
        )
        {
            return new AppResponseModel { AppCode = Constant.AppCode.VALIDATION_FAILED };
        }

        // Tạo mới hoặc cập nhật history
        if (string.IsNullOrWhiteSpace(historyId))
        {
            var userId = _httpContextAccessor.Value.HttpContext.User.FindFirstValue("sub");
            if (userId != null)
            {
                var newHistoryId = await _repository.Value.createAnHistory(
                    prompt,
                    geminiResponse.planUML,
                    Guid.Parse(userId),
                    cancellationToken
                );
                historyId = newHistoryId.ToString();
            }
        }
        else
        {
            await _repository.Value.updateHistory(
                geminiResponse.planUML,
                Guid.Parse(historyId),
                cancellationToken
            );
        }

        // Lưu prompt và phản hồi vào lịch sử
        await _repository.Value.saveMessageToHistory(
            Guid.Parse(historyId),
            prompt,
            "request",
            cancellationToken
        );

        await _repository.Value.saveMessageToHistory(
            Guid.Parse(historyId),
            geminiResponse.response_text,
            "response",
            cancellationToken
        );

        return new AppResponseModel
        {
            AppCode = Constant.AppCode.SUCCESS,
            Body = new()
            {
                PlantUML = geminiResponse.planUML,
                ResponseText = geminiResponse.response_text,
                HistoryId = historyId,
            }
        };
    }

}

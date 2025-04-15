using Base.Gemini.Handler;
using FCommon.AccessToken;
using FCommon.Constants;
using FCommon.FeatureService;
using FCommon.RefreshToken;
using Microsoft.AspNetCore.Http;
using PromptToAI.Common;
using PromptToAI.DataAccess;
using PromptToAI.Models;
using System.Security.Claims;

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
            return new AppResponseModel
            {
                AppCode = Constant.AppCode.VALIDATION_FAILED
            };
        }
        var historyId = request.HistoryId;
        var prompt = request.Prompt;
        // Tạo danh sách MessageType để gửi cho Gemini
        var messages = new List<Base.Config.MessageType>
        {
            new Base.Config.MessageType
            {
                Role = "user",
                Parts = new List<Base.Config.Part>
                {
                    new Base.Config.Part
                    {
                        text = prompt
                    }
                }
            }
        };

        // Gọi tới GeminiService để lấy phản hồi từ AI
        var geminiResponse = await _geminiService.Value.PostAnswerAsync(messages);

        if (geminiResponse == null ||
            string.IsNullOrWhiteSpace(geminiResponse.planUML) ||
            string.IsNullOrWhiteSpace(geminiResponse.response_text))
        {
            return new AppResponseModel
            {
                AppCode = Constant.AppCode.VALIDATION_FAILED
            };
        }

        if (historyId == null)
        {
            var userId = _httpContextAccessor.Value.HttpContext.User.FindFirstValue(claimType: "sub");
            if (userId != null) historyId = (await _repository.Value.createAnHistory(prompt, geminiResponse.planUML, Guid.Parse(userId!), cancellationToken)).ToString();
        } else
        {
            await _repository.Value.updateHistory(geminiResponse.planUML, Guid.Parse(historyId), cancellationToken);
        }
        await _repository.Value.saveMessageToHistory(Guid.Parse(historyId), prompt, "request", cancellationToken);
        await _repository.Value.saveMessageToHistory(Guid.Parse(historyId), geminiResponse.response_text, "response", cancellationToken);

        return new AppResponseModel
        {
            AppCode = Constant.AppCode.SUCCESS,
            Body = new()
            {
                PlantUML = geminiResponse.planUML,
                ResponseText = geminiResponse.response_text,
                HistoryId = historyId
            }
        };
    }
}

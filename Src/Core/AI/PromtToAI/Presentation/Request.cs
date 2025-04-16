using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PromptToAI.Presentation;

[ValidateNever]
public sealed class Request
{
    public string? HistoryId { get; set; }
    public string Prompt { get; set; }
}

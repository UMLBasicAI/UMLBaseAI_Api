using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DeletePromptHistoryById.Presentation;

[ValidateNever]
public sealed class Request
{
    [FromRoute] //params
    public string HistoryId { get; set; }
}

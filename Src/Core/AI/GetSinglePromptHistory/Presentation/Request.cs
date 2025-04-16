using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GetSinglePromptHistory.Presentation;

[ValidateNever]
public sealed class Request
{
    [FromQuery]
    [Range(1, int.MaxValue, ErrorMessage = "Page must be a positive integer.")]
    public int Page { get; set; }

    [FromQuery]
    [Range(1, int.MaxValue, ErrorMessage = "Size must be a positive integer.")]
    public int Size { get; set; }

    [FromRoute]
    [Required(ErrorMessage = "HistoryId is required.")]
    public string HistoryId { get; set; }
}


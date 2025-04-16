using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GetPromptHistories.Presentation;

public sealed class Request
{
    [FromQuery]
    [Range(1, int.MaxValue, ErrorMessage = "Page must be a positive integer.")]
    public int Page { get; set; }

    [FromQuery]
    [Range(1, int.MaxValue, ErrorMessage = "Size must be a positive integer.")]
    public int Size { get; set; }
}

using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DeletePromptHistoryById.Presentation;

[ValidateNever]
public sealed class Request
{
    [DefaultValue("vuvo070403@gmail.com")]
    public string Email { get; set; }

    [DefaultValue("Admin123@")]
    public string Password { get; set; }
}

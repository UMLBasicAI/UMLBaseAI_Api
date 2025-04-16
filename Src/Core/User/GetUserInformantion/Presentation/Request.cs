using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GetUserInformation.Presentation;

[ValidateNever]
public sealed class Request
{
    public bool? FlagEmptyRequest { get; set; }
}

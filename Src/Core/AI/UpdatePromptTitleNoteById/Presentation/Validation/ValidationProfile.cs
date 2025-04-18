﻿using Base.Config;
using FluentValidation;

namespace UpdatePromptTitleNoteById.Presentation.Validation;

public sealed class ValidationProfile : AbstractValidator<Request>
{
    public ValidationProfile(AspNetCoreIdentityOption aspNetCoreIdentityOption)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.HistoryId).NotEmpty();
        RuleFor(x => x.NewAction).NotEmpty();
    }
}

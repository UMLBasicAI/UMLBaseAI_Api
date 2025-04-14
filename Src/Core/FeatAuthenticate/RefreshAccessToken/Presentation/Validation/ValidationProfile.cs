﻿using Base.Config;
using FluentValidation;

namespace RefreshAccessToken.Presentation.Validation;

public sealed class ValidationProfile : AbstractValidator<Request>
{
    public ValidationProfile(AspNetCoreIdentityOption aspNetCoreIdentityOption)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
    }
}

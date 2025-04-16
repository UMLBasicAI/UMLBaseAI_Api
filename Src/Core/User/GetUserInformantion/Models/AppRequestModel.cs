﻿using FCommon.FeatureService;

namespace GetUserInformation.Models;

public sealed class AppRequestModel : IServiceRequest<AppResponseModel>
{
    public bool? FlagEmptyRequest { get; set; }
}

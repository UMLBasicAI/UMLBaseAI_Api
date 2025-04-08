﻿using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignIn.BusinessLogic;
using SignIn.Common;
using SignIn.Mapper;
using SignIn.Models;
using SignIn.Presentation;
using SignIn.Presentation.Filter.SetStageBag;
using SignIn.Presentation.Validation;

namespace SignUp.Presentation;

[Tags(Constant.CONTROLLER_NAME)]
public sealed class Endpoint : ControllerBase
{
    private readonly Service _service;

    public Endpoint(Service service) => _service = service;

    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(1, Type = typeof(Response))]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [HttpPost(Constant.ENDPOINT_PATH)]
    [ServiceFilter<SetStageBagFilter>]
    [ServiceFilter<ValidationFilter>]
    public async Task<IActionResult> ExecuteAsync(
        [FromBody] [Required] Request request,
        CancellationToken cancellationToken
    )
    {
        var appRequest = new AppRequestModel
        {
            Email = request.Email,
            Password = request.Password,
            RememberMe = request.RememberMe,
        };
        var appResponse = await _service.ExecuteAsync(appRequest, cancellationToken);
        var httpResponse = HttpResponseMapper.Get(appRequest, appResponse, HttpContext);
        return StatusCode(httpResponse.HttpCode, httpResponse);
    }
}

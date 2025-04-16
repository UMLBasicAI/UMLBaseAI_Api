using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using FCommon.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GetSinglePromptHistory.BusinessLogic;
using GetSinglePromptHistory.Common;
using GetSinglePromptHistory.Mapper;
using GetSinglePromptHistory.Models;
using GetSinglePromptHistory.Presentation.Filter.SetStageBag;
using GetSinglePromptHistory.Presentation.Validation;

namespace GetSinglePromptHistory.Presentation;

[Tags(Constant.CONTROLLER_NAME)]
public sealed class Endpoint : ControllerBase
{
    private readonly Service _service;

    public Endpoint(Service service) => _service = service;

    /// <summary>
    ///     Endpoint for user login.
    /// </summary>
    /// <param name="request">
    ///     Incoming request.
    /// </param>
    /// <response code="429">TEMPORARY_BANNED</response>
    /// <response code="401">PASSWORD_IS_INCORRECT</response>
    /// <response code="404">USER_NOT_FOUND</response>
    /// <response code="400">VALIDATION_FAILED</response>
    /// <response code="500">SERVER_ERROR</response>
    /// <response code="200">SUCCESS</response>
    /// <response code="1">EXAMPLE RESPONSE OF ALL STATUS CODES</response>
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(1, Type = typeof(Response))]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [HttpGet(Constant.ENDPOINT_PATH)]
    [Authorize(Policy = nameof(DefaultAuthorizationRequirement))]
    [ServiceFilter<SetStageBagFilter>]
    [ServiceFilter<ValidationFilter>]
    public async Task<IActionResult> ExecuteAsync(
        [Required] Request request,
        CancellationToken cancellationToken
    )
    {
        var appRequest = new AppRequestModel
        {
            HistoryId = request.HistoryId,
            Page = request.Page,
            Size = request.Size,
        };
        var appResponse = await _service.ExecuteAsync(appRequest, cancellationToken);
        var httpResponse = HttpResponseMapper.Get(appRequest, appResponse, HttpContext);
        return StatusCode(httpResponse.HttpCode, httpResponse);
    }
}

using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DeletePromptHistoryById.BusinessLogic;
using DeletePromptHistoryById.Common;
using DeletePromptHistoryById.Mapper;
using DeletePromptHistoryById.Models;
using DeletePromptHistoryById.Presentation.Filter.SetStageBag;
using DeletePromptHistoryById.Presentation.Validation;
using FCommon.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace DeletePromptHistoryById.Presentation;

[Tags(Constant.CONTROLLER_NAME)]
public sealed class Endpoint : ControllerBase
{
    private readonly Service _service;

    public Endpoint(Service service) => _service = service;

    /// <summary>
    ///     Endpoint for register new user.
    /// </summary>
    /// <param name="request">
    ///     Incoming request.
    /// </param>
    /// <response code="422">PASSWORD_IS_INVALID</response>
    /// <response code="409">EMAIL_ALREADY_EXISTS</response>
    /// <response code="400">VALIDATION_FAILED</response>
    /// <response code="500">SERVER_ERROR</response>
    /// <response code="200">SUCCESS</response>
    /// <response code="1">EXAMPLE RESPONSE OF ALL STATUS CODES</response>
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(1, Type = typeof(Response))]
    [Produces(MediaTypeNames.Application.Json)]
    [HttpDelete(Constant.ENDPOINT_PATH)]
    [Authorize(Policy = nameof(DefaultAuthorizationRequirement))]
    [ServiceFilter<SetStageBagFilter>]
    [ServiceFilter<ValidationFilter>]
    public async Task<IActionResult> ExecuteAsync(
        [FromRoute] [Required] Request request,
        CancellationToken cancellationToken
    )
    {
        var appRequest = new AppRequestModel { HistoryId = request.HistoryId};
        
        var appResponse = await _service.ExecuteAsync(appRequest, cancellationToken);

        var httpResponse = HttpResponseMapper.Get(appRequest, appResponse, HttpContext);
        return StatusCode(httpResponse.HttpCode, httpResponse);
    }
}

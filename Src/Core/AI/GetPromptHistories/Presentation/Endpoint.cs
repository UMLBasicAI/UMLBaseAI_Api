using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using FCommon.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GetPromptHistories.BusinessLogic;
using GetPromptHistories.Common;
using GetPromptHistories.Mapper;
using GetPromptHistories.Models;
using GetPromptHistories.Presentation.Filter.SetStageBag;
using GetPromptHistories.Presentation.Validation;

namespace GetPromptHistories.Presentation;

[Tags(Constant.CONTROLLER_NAME)]
public sealed class Endpoint : ControllerBase
{
    private readonly Service _service;

    public Endpoint(Service service) => _service = service;

    /// <summary>
    ///     Endpoint for retrieving prompt histories for a user.
    /// </summary>
    /// <param name="request">
    ///     Incoming request for retrieving the history.
    /// </param>
    /// <response code="429">TEMPORARY_BANNED</response>
    /// <response code="401">USER_NOT_FOUND</response>
    /// <response code="400">VALIDATION_FAILED</response>
    /// <response code="500">SERVER_ERROR</response>
    /// <response code="200">SUCCESS</response>
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
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
            Page = request.Page,
            Size = request.Size
        };

        // Gọi service để lấy dữ liệu lịch sử
        var appResponse = await _service.ExecuteAsync(appRequest, cancellationToken);

        // Sử dụng HttpResponseMapper để tạo HTTP response
        var httpResponse = HttpResponseMapper.Get(appRequest, appResponse, HttpContext);

        // Trả về response với mã trạng thái và body tương ứng
        return StatusCode(httpResponse.HttpCode, httpResponse);
    }
}

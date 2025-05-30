using Microsoft.Teams.Assist.Application.Common.Models;
using Microsoft.Teams.Assist.Application.Email;
using Microsoft.Teams.Assist.Application.Email.Model.Request;
using Microsoft.Teams.Assist.Application.Email.Model.Response;
using Microsoft.Teams.Assist.Host.Controllers.BaseControllers;

namespace Microsoft.Teams.Assist.Host.Controllers.EmailLog;
public class EmailLogController : VersionNeutralApiController
{
    #region Properties
    public readonly IEmailLogService _emailLogService;
    #endregion
    #region Constructor
    public EmailLogController(IEmailLogService emailLogService)
    {
        _emailLogService = emailLogService;
    }

    #endregion
    #region Methods

    /// <summary>
    /// Get email log  records by filter.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("get-email-logs")]
    [MustHavePermission(SystemAction.View, SystemResource.EmailLog)]
    [OpenApiBodyParameter("Get all email-logs", "")]
    public Task<PaginationResponse<ViewEmailLogResponse>> GetEmailLogList(SearchEmailLogRequest request)
    {
        return _emailLogService.GetEmailLogAsync(request);
    }

    /// <summary>
    /// Get email log by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("get-email-log/{id}")]
    [MustHavePermission(SystemAction.View, SystemResource.EmailLog)]
    [OpenApiOperation("Get email log by Id", "")]
    public Task<ViewEmailLogDetailResponse> GetEmailLogById(int id)
    {
        return _emailLogService.GetEmailLogByIdAsync(id);
    }
    #endregion

}

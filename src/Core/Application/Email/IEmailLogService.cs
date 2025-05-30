using Microsoft.Teams.Assist.Application.Common.Models;
using Microsoft.Teams.Assist.Application.Email.Model.Request;
using Microsoft.Teams.Assist.Application.Email.Model.Response;
using Microsoft.Teams.Assist.Domain.Email;

namespace Microsoft.Teams.Assist.Application.Email;
public interface IEmailLogService : ITransientService
{
    Task AddEmailLogAsync(EmailLog emailLog);
    Task<ViewEmailLogDetailResponse> GetEmailLogByIdAsync(int id);
    Task<PaginationResponse<ViewEmailLogResponse>> GetEmailLogAsync(SearchEmailLogRequest request);
}

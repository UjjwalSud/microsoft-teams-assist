using Microsoft.Teams.Assist.Application.Common.Models;
using Microsoft.Teams.Assist.Application.Email;
using Microsoft.Teams.Assist.Application.Email.Model.Request;
using Microsoft.Teams.Assist.Application.Email.Model.Response;
using Microsoft.Teams.Assist.Domain.Email;
using Microsoft.Teams.Assist.Infrastructure.Persistence.Context;
using Microsoft.Teams.Assist.Infrastructure.SystemConstants;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Teams.Assist.Infrastructure.Orbit.Email;
internal class EmailLogService : IEmailLogService
{
    private readonly ApplicationDbContext _context;
    public EmailLogService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddEmailLogAsync(EmailLog emailLog)
    {
        await _context.EmailLog.AddAsync(emailLog);
        await _context.SaveChangesAsync();
    }

    public async Task<PaginationResponse<ViewEmailLogResponse>> GetEmailLogAsync(SearchEmailLogRequest request)
    {
        var query = _context.EmailLog.AsQueryable();
        if (request.To != null)
        {
            query = query.Where(x => x.To == request.To);
        }

        if (request.Subject != null)
        {
            query = query.Where(x => x.Subject == request.Subject);
        }

        if (request.SentStatus != null)
        {
            query = query.Where(x => x.IsEmailSent == request.SentStatus);
        }

        if (request.SendDateTime.HasValue)
        {
            query = query.Where(x => x.CreatedOn == request.SendDateTime.Value);
        }

        return await query.PaginatedListAsync<EmailLog, ViewEmailLogResponse>(request.PageNumber, request.PageSize);
    }

    public async Task<ViewEmailLogDetailResponse> GetEmailLogByIdAsync(int id)
    {
        var entity = await _context.EmailLog.SingleOrDefaultAsync(x => x.Id == id);
        _ = entity ?? throw new NotImplementedException(string.Format(ErrorMessages.ItemNotFound, "Item"));
        return entity.Adapt<ViewEmailLogDetailResponse>();
    }
}

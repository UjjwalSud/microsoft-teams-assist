using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Teams.Assist.Application.Common.Exceptions;
using Microsoft.Teams.Assist.Application.Common.Models;
using Microsoft.Teams.Assist.Application.Common.Models.Response;
using Microsoft.Teams.Assist.Application.LookUp;
using Microsoft.Teams.Assist.Application.LookUp.Models.Request;
using Microsoft.Teams.Assist.Application.LookUp.Models.Response;
using Microsoft.Teams.Assist.Domain.Enums;
using Microsoft.Teams.Assist.Domain.LookUp;
using Microsoft.Teams.Assist.Infrastructure.Persistence.Context;
using Microsoft.Teams.Assist.Infrastructure.SystemConstants;

namespace Microsoft.Teams.Assist.Infrastructure.Orbit.LookUp;
public class LookUpService : ILookUpService
{
    private readonly ApplicationDbContext _applicationDbContext;
    public LookUpService(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<List<ViewLookUpsResponse>> GetLookUpCodesAsync()
    {
        var result = await _applicationDbContext.LookUpCodes.ToListAsync();
        return result.Adapt<List<ViewLookUpsResponse>>();
    }

    public async Task<PaginationResponse<ViewLookUpCodeValuesResponse>> GetLookUpCodeValuesAsync(SearchLookUpCodeValuesRequest request)
    {
        return await _applicationDbContext.LookUpCodeValues.Where(x => x.LookUpCode.LookUpCodeType == request.Type).PaginatedListAsync
            <LookUpCodeValues, ViewLookUpCodeValuesResponse>(request.PageNumber, request.PageSize);
    }

    public async Task<List<DropDownItemResponse>> GetLookUpCodeValuesByTypeAsync(LookUpCodeTypes type)
    {
        var result = await _applicationDbContext.LookUpCodeValues.Where(x => x.LookUpCode.LookUpCodeType == type).ToListAsync();

        return result.OrderBy(x => x.DisplayOrder).ThenBy(x => x.LookUpValue).Select(x => new DropDownItemResponse
        {
            Value = x.Id,
            Text = x.LookUpValue
        }).ToList();
    }


    public async Task<string> CreateLookUpCodeValueAsync(CreateLookUpCodeValueRequest request)
    {
        var existingItem = await _applicationDbContext.LookUpCodeValues.SingleOrDefaultAsync(x => x.FKLookUpCodePKId == request.LookUpCodeId && x.LookUpValue == request.LookUpValue);
        if (existingItem != null)
        {
            throw new ConflictException(string.Format(ErrorMessages.ItemAlreadyExists, request.LookUpValue));
        }

        await _applicationDbContext.LookUpCodeValues.AddAsync(new LookUpCodeValues
        {
            LookUpValue = request.LookUpValue,
            DisplayOrder = request.DisplayOrder,
            FKLookUpCodePKId = request.LookUpCodeId,
            IsActive = request.IsActive,
        });

        await _applicationDbContext.SaveChangesAsync();

        return SuccessMessages.RecordAddedSuccessfully;
    }

    public async Task<string> UpdateLookUpCodeValueAsync(UpdateLookUpCodeValueRequest request)
    {
        var entity = await _applicationDbContext.LookUpCodeValues.SingleOrDefaultAsync(x => x.Id == request.Id);
        _ = entity ?? throw new NotFoundException(string.Format(ErrorMessages.ItemNotFound, "Item"));

        var existingItem = await _applicationDbContext.LookUpCodeValues.SingleOrDefaultAsync(x => x.FKLookUpCodePKId == entity.FKLookUpCodePKId
        && x.Id != request.Id
        && x.LookUpValue == request.LookUpValue);
        if (existingItem != null)
        {
            throw new ConflictException(string.Format(ErrorMessages.ItemAlreadyExists, request.LookUpValue));
        }

        entity.LookUpValue = request.LookUpValue;
        entity.DisplayOrder = request.DisplayOrder;
        entity.IsActive = request.IsActive;
        _applicationDbContext.LookUpCodeValues.Update(entity);

        await _applicationDbContext.SaveChangesAsync();

        return SuccessMessages.RecordUpdatedSuccessfully;
    }

    public async Task<ViewLookUpCodeValuesResponse> GetLookUpCodeValueByIdAsync(int id)
    {
        var entity = await _applicationDbContext.LookUpCodeValues.SingleOrDefaultAsync(x => x.Id == id);
        _ = entity ?? throw new NotFoundException(string.Format(ErrorMessages.ItemNotFound, "Item"));

        return entity.Adapt<ViewLookUpCodeValuesResponse>();
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Teams.Assist.Application.Common.Exceptions;
using Microsoft.Teams.Assist.Application.Common.Interfaces;
using Microsoft.Teams.Assist.Application.Setting;
using Microsoft.Teams.Assist.Application.Setting.Models.Request;
using Microsoft.Teams.Assist.Application.Setting.Models.Response;
using Microsoft.Teams.Assist.Infrastructure.Persistence.Context;
using Microsoft.Teams.Assist.Infrastructure.SystemConstants;

namespace Microsoft.Teams.Assist.Infrastructure.Orbit.Setting;
internal class SettingService : ISettingService
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ISerializerService _serializerService;

    public SettingService(ApplicationDbContext applicationDbContext, ISerializerService serializerService)
    {
        _applicationDbContext = applicationDbContext;
        _serializerService = serializerService;
    }

    public async Task<List<ViewSettingResponse>> GetSettingsAsync()
    {
        var result = await _applicationDbContext.Settings.ToListAsync();

        return result.ConvertAll(x => new ViewSettingResponse
        {
            Description = x.Description,
            Id = x.Id,
            SettingType = x.SettingType,
        });
    }

    public async Task<ViewSettingResponse> GetById(int id)
    {
        var entity = await _applicationDbContext.Settings.SingleOrDefaultAsync(x => x.Id == id);
        _ = entity ?? throw new NotFoundException(string.Format(ErrorMessages.ItemNotFound, "Item"));

        return new ViewSettingResponse
        {
            SettingType = entity.SettingType,
            Description = entity.Description,
            Id = entity.Id,
        };
    }

    public async Task<ViewSettingDetailResponse<T>> GetSettingByIdAsync<T>(int id)
    {
        var entity = await _applicationDbContext.Settings.SingleOrDefaultAsync(x => x.Id == id);
        _ = entity ?? throw new NotFoundException(string.Format(ErrorMessages.ItemNotFound, "Item"));

        switch (entity.SettingType)
        {

            default:
                throw new CustomNotImplementedException($"{entity.SettingType} not implemented");
        }
    }

    public async Task<string> UpdateSettings(UpdateSettingsRequest request)
    {
        var entity = await _applicationDbContext.Settings.SingleOrDefaultAsync(x => x.Id == request.Id);
        _ = entity ?? throw new NotFoundException(string.Format(ErrorMessages.ItemNotFound, "Item"));
        try
        {
            switch (entity.SettingType)
            {
              
                default:
                    throw new CustomNotImplementedException($"{entity.SettingType} not implemented");
            }
        }
        catch
        {
            throw new BadRequestException(ErrorMessages.UpdateSettingsDeserializeCrash);
        }

        entity.SettingValues = request.SettingJson;
        _applicationDbContext.Settings.Update(entity);
        await _applicationDbContext.SaveChangesAsync();

        return SuccessMessages.RecordUpdatedSuccessfully;
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Teams.Assist.Application.Common.Exceptions;
using Microsoft.Teams.Assist.Application.Common.Interfaces;
using Microsoft.Teams.Assist.Application.Nexus.Subscription;
using Microsoft.Teams.Assist.Application.Nexus.Subscription.Models;
using Microsoft.Teams.Assist.Domain.Enums;
using Microsoft.Teams.Assist.Application.Common.Extensions;
using Microsoft.Teams.Assist.Infrastructure.Nexus.Subscription.DbModels;
using Microsoft.Teams.Assist.Infrastructure.Persistence.Context.Nexus;
using Microsoft.Teams.Assist.Infrastructure.SystemConstants;

namespace Microsoft.Teams.Assist.Infrastructure.Nexus.Subscription;
public class SubscriptionService : ISubscriptionService
{
    private readonly NexusDbContext _nexusDbContext;
    private readonly ISerializerService _serializerService;
    public SubscriptionService(NexusDbContext nexusBaseDbContext, ISerializerService serializerService)
    {
        _nexusDbContext = nexusBaseDbContext;
        _serializerService = serializerService;
    }

    public SubscriptionRulesDto GetSubscriptionDefaultSetting(SubscriptionTypes subscriptionType)
    {
        switch (subscriptionType)
        {
            case SubscriptionTypes.First:
                return new SubscriptionRulesDto { MaxUsers = 1 };
            case SubscriptionTypes.Second:
                return new SubscriptionRulesDto {MaxUsers = 2 };
            case SubscriptionTypes.Third:
                return new SubscriptionRulesDto {MaxUsers = 3 };
            case SubscriptionTypes.Fourth:
                return new SubscriptionRulesDto {MaxUsers = 4 };
            case SubscriptionTypes.Fifth:
                return new SubscriptionRulesDto {MaxUsers = 5 };
            default:
                throw new Exception($"{subscriptionType.GetDescription()} not implemented");
        }
    }

    public SubscriptionTypes GetRootAdminSubscription()
    {
        return SubscriptionTypes.Fifth;
    }

    public async Task<int> GetRegisteredUserDefaultSubscriptionAsync()
    {
        return (await _nexusDbContext.Subscriptions.SingleOrDefaultAsync(x => x.SubscriptionType == SubscriptionTypes.First)).Id;
    }

    public async Task<SubscriptionRulesDto> SubscriptionRulesByIdAsync(int id)
    {
        var item = await GetByIdAsync(id);

        _ = item ?? throw new NotFoundException(string.Format(ErrorMessages.ItemNotFound, "Role"));

        return _serializerService.Deserialize<SubscriptionRulesDto>(item.Setting);
    }

    private async Task<Subscriptions> GetByIdAsync(int id)
    {
        var item = await _nexusDbContext.Subscriptions.SingleOrDefaultAsync(x => x.Id == id);

        _ = item ?? throw new NotFoundException(string.Format(ErrorMessages.ItemNotFound, "Role"));

        return item;
    }
}

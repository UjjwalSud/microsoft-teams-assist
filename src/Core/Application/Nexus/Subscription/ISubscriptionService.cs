using Microsoft.Teams.Assist.Application.Nexus.Subscription.Models;
using Microsoft.Teams.Assist.Domain.Enums;

namespace Microsoft.Teams.Assist.Application.Nexus.Subscription;
public interface ISubscriptionService : ITransientService
{
    SubscriptionRulesDto GetSubscriptionDefaultSetting(SubscriptionTypes subscriptionType);
    SubscriptionTypes GetRootAdminSubscription();
    Task<int> GetRegisteredUserDefaultSubscriptionAsync();
    Task<SubscriptionRulesDto> SubscriptionRulesByIdAsync(int id);
}

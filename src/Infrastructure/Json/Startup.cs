using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.Teams.Assist.Infrastructure.Json;
public static class Startup
{
    public static IMvcBuilder SetJsonSerializerOptions(this IMvcBuilder services)
    {
        return services.AddJsonOptions(options =>
          {
              // Ensure DateTime is formatted as ISO 8601
              options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
              options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
              options.JsonSerializerOptions.WriteIndented = false;
              options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
              options.JsonSerializerOptions.Converters.Add(new JsonDateTimeConverter());
          });
    }
}


using Mapster;
using WebAPITemplate.Shared.Models.Entities;
using WebAPITemplate.Shared.Models.Requests.User;

namespace WebAPITemplate.API.Extensions;

public static class MapsterExtension
{
    public static void RegisterMapsterConfiguration(this IServiceCollection services)
    {
        TypeAdapterConfig<UserUpdateRequest, User>.NewConfig().IgnoreNullValues(true);
    }
}
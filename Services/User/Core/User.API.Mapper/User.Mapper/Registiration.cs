using Microsoft.Extensions.DependencyInjection;
using User.Application.Interfaces.AutoMapper;

namespace User.Mapper
{
    public static class Registration
    {
        public static void AddCustomMapper(this IServiceCollection services)
        {
            services.AddSingleton<IMapper, AutoMapper.Mapper>();
        }
    }
}

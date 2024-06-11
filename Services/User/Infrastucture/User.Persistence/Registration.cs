using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.Interfaces.RedisCache;
using User.Application.Interfaces.Repositories;
using User.Application.Interfaces.Services;
using User.Application.Interfaces.UnitOfWorks;
using User.Application.Services.Commands;
using User.Application.Services.Querires;
using User.Domain.Entities;
using User.Persistence.Context;
using User.Persistence.Repositories;
using User.Persistence.UnitOfWorks;

namespace User.Persistence
{
    public static class Registration
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
            services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));
            services.AddScoped<IUserCommandService, UserCommandService>();
            services.AddScoped<IUserQueryService, UserQueryService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddIdentityCore<Domain.Entities.User>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 2;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireDigit = false;
                opt.SignIn.RequireConfirmedEmail = false;
            })
            .AddRoles<Domain.Entities.Role>()
            .AddEntityFrameworkStores<AppDbContext>();
        }


    }

}

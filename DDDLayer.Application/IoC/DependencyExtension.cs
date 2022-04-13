using DDDLayer.Application.Services;
using DDDLayer.Application.Services.Abstract;
using DDDLayer.Application.Services.Interfaces;
using DDDLayer.Shared.Interfaces;
using DDDLayer.Shared.Repositories;
using DDDLayer.Shared.Uow;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDLayer.Application.IoC
{
    public static class DependencyExtension
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<DbConnection>(p => new SqlConnection(configuration.GetConnectionString("DefaultConnection"))).AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();



        }
    }
}

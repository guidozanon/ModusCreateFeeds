using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModusCreate.Core.Infrastructure;
using ModusCreate.Web.Configuration;
using ModusCreate.Web.Infrastructure;
using ModusCreate.Web.Secutiry;

namespace ModusCreate.Web
{
    public static class WebApiRegistry
    {
        private const string JwtAuthenticationKey = "JwtAuthentication";
        private const string GlobalConfigurationKey = "GlobalConfiguration";
        public static void AddWebApiRegistry(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtAuthenticationConfiguration>(configuration.GetSection(JwtAuthenticationKey));
            services.Configure<GlobalConfiguration>(configuration.GetSection(GlobalConfigurationKey));
            services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();

            services.AddSingleton<ITokenGenerator, JwtTokenGenerator>();
            services.AddTransient<IInstallerEngine, InstallerEngine>();

            services.AddAutoMapper();

            services.Scan(scan => scan
                .FromAssemblyOf<Startup>()
                .AddClasses(classes => classes.AssignableTo<IInstaller>())
                .AsImplementedInterfaces()
                .WithTransientLifetime()
            );
        }
    }
}

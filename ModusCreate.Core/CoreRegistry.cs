using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ModusCreate.Core.DAL;
using ModusCreate.Core.DAL.Domain;
using ModusCreate.Core.DAL.Repositories;
using ModusCreate.Core.Infrastructure;
using ModusCreate.Core.Models;
using ModusCreate.Core.Services;

namespace ModusCreate.Core
{
    public static class CoreRegistry
    {
        public static void AddCoreRegistry(this IServiceCollection services, string connString)
        {
            services.AddScoped<NewsFeedContext>();
            services.AddScoped<IFeedsService, FeedService>();
            services.AddScoped<IInternalUserService, UserService>();
            services.AddScoped<IUserService>(x => x.GetRequiredService<IInternalUserService>());


            services.AddDbContext<NewsFeedContext>(options =>
                options.UseSqlServer(connString)
            );

            services.AddDefaultIdentity<UserEntity>()
                .AddEntityFrameworkStores<NewsFeedContext>();

            services.AddScoped<IRepository<Feed>, Repository<FeedEntity, Feed>>();
            services.AddScoped<IRepository<News>, Repository<NewsEntity, News>>();



            services.Scan(scan => scan
                .FromAssemblyOf<NewsFeedContext>()
                .AddClasses(classes => classes.AssignableTo<IInstaller>())
                .AsImplementedInterfaces()
                .WithTransientLifetime()
                );
        }
    }
}

using log4net;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ModusCreate.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModusCreate.Web.Infrastructure
{
    public interface IInstallerEngine
    {
        Task Install();
    }

    public class InstallerEngine : IInstallerEngine
    {
        private readonly static ILog _logger = LogManager.GetLogger(typeof(InstallerEngine));
        private readonly IEnumerable<IInstaller> _installers;

        public InstallerEngine(IEnumerable<IInstaller> installers)
        {
            _installers = installers;
        }

        public async Task Install()
        {
            foreach (var installer in _installers)
            {
                try
                {
                    _logger.Info($"Start running installer ${installer.GetType().Name}");

                    await installer.Install();

                    _logger.Info($"Installer ${installer.GetType().Name} completed");
                }
                catch (AggregateException ex)
                {
                    _logger.Fatal(ex.InnerExceptions);
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.Fatal(ex);
                    throw;
                }
            }
        }
    }

    public static class InstallerExtensions
    {
        public static void RunInstallers(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var installerEngine = scope.ServiceProvider.GetService<IInstallerEngine>();

                installerEngine.Install().Wait();
            }
        }
    }
}

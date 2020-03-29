using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IisConfiguration;
using IisConfiguration.Logging;
using Microsoft.Web.Administration;

namespace RealEstateAggregator.IisConfigTool
{
    class Program
    {
        private static readonly ConsoleLogger Logger = new ConsoleLogger();
        private static readonly WebServerConfig WebServerConfig = new WebServerConfig(Logger);
        private static readonly Config EnvConfig = new Config();

        static void Main()
        {
            if (!WebServerConfig.IsIis7OrAbove)
            {
                Logger.LogHeading("IIS7 is not installed on this machine. IIS configuration setup terminated.");
                return;
            }

            try
            {
                Logger.Log("Setting up RealEstateAggregator.Web");
                AddWebApp();
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }

            if (Debugger.IsAttached)
            {
                Logger.Log("Debugger attached, hit any key to quit");
                Console.ReadKey();
            }
        }

        private static void AddWebApp()
        {
            WebServerConfig
                .AddAppPool(EnvConfig.WebAppName, "", ManagedPipelineMode.Integrated, EnvConfig.AppPoolIdentityType)
                .WithProcessModel(EnvConfig.IdleTimeout, EnvConfig.PingingEnabled)
                .WithCredentials(EnvConfig.AppPoolUser, EnvConfig.AppPoolPassword)
                .Commit();

            WebServerConfig
                .AddSite(EnvConfig.WebAppName, EnvConfig.WebPort, EnvConfig.WebPort)
                .AddApplication("/", EnvConfig.WebRoot, EnvConfig.WebAppName)
                .Commit();
        }
    }
}

using RamDam.BackEnd.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;

namespace RamDam.BackEnd.Core.Utilities
{
    public static class LoggerFactoryExtensions
    {
        public static ILoggerFactory AddSerilog(
            this ILoggerFactory factory,
            IWebHostEnvironment env,
            IHostApplicationLifetime appLifetime,
            GlobalSettings globalSettings,
            Func<LogEvent, bool> filter = null)
        {
            //if (!env.IsDevelopment())
            //{
                if (filter == null)
                {
                    filter = (e) => true;
                }

                var config = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .Filter.ByIncludingOnly(filter);


                if (CoreHelpers.SettingHasValue(globalSettings.LogDirectory))
                {
                    config.WriteTo.RollingFile($"{globalSettings.LogDirectory}/{{Date}}.log",
                        outputTemplate: "{Timestamp:HH:mm:ss} [{Level}] {UserId} - {Message}{NewLine}{Exception}");
                }

                var serilog = config.CreateLogger();
                factory.AddSerilog(serilog);
                appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);
            //}

            return factory;
        }
    }
}

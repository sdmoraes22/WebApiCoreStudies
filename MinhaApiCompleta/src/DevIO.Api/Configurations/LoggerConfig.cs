using System;
using Elmah.Io.AspNetCore;
using Elmah.Io.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DevIO.Api.Configurations
{
    public static class LoggerConfig
    {
        public static IServiceCollection AddLoggerConfiguration(this IServiceCollection services)
        {
            services.AddElmahIo(o =>
            {
                o.ApiKey = "b9e1d08cfdda465885f6fdea1ba984ad";
                o.LogId = new Guid("344536ce-6ca0-4e40-98ad-274a0209035b");
            });

            // services.AddLogging(builder => 
            // {
            //     builder.AddElmahIo(o =>
            //     {
            //         o.ApiKey = "b9e1d08cfdda465885f6fdea1ba984ad";
            //         o.LogId = new Guid("344536ce-6ca0-4e40-98ad-274a0209035b");
            //     });
            //     builder.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Warning);
            // });
            return services;
        }

        public static IApplicationBuilder UseLoggerConfiguration(this IApplicationBuilder app)
        {
            app.UseElmahIo();
            return app;
        }
    }
}
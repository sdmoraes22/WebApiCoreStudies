using System;
using DevIO.Api.Extensions;
using Elmah.Io.AspNetCore;
using Elmah.Io.AspNetCore.HealthChecks;
using Elmah.Io.Extensions.Logging;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DevIO.Api.Configurations
{
    public static class LoggerConfig
    {
        
        public static IServiceCollection AddLoggerConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddElmahIo(o =>
            {
                o.ApiKey = "[MyApiKey]";
                o.LogId = new Guid("[MyLogGuid]");
            });

            services.AddHealthChecks()
                .AddElmahIoPublisher("[MyApiKet]", new Guid("[MyLogGuid]")
                .AddCheck("produtos", new MySqlHealthCkeck(configuration.GetConnectionString("DefaultConnection")))
                .AddMySql(configuration.GetConnectionString("DefaultConnection"), "BancoMySql");
                
            
            services.AddHealthChecksUI();            

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
            app.UseHealthChecks("/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.UseHealthChecksUI(options => 
            { 
                options.UIPath = "/ui";
            });
            
            app.UseElmahIo();
            return app;
        }
    }
}
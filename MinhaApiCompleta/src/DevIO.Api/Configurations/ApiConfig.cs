using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace DevIO.Api.Configurations
{
    public static class ApiConfig
    {
        public static IServiceCollection WebApiConfig(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options => 
            {
                options.SuppressModelStateInvalidFilter = true;
                
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddApiVersioning(options => 
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options => 
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("Development",
                            builder => 
                                builder
                                    .AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader()
                                    .AllowCredentials());
                
                options.AddPolicy("Production",
                            builder => 
                                builder
                                    .WithMethods("GET")
                                    .WithOrigins("http://localhost")
                                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                                    // .WithHeaders(HeaderNames.ContentType, "x-custom-header")
                                    .AllowAnyHeader());
            });
            return services;
        }

        public static IApplicationBuilder UseMvcConfiguration(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseCors("Development");
            app.UseMvc();
            return app;
        }
    }
}
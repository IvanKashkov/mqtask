using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO.Compression;
using mqtask.Application;
using mqtask.Domain;
using Microsoft.AspNetCore.ResponseCompression;

namespace mqtask.WebApi
{
    public class Startup
    {
        /// <summary>
        /// This is just a flag for switching possible solutions
        /// </summary>
        private static bool UseControllersWithResponseCaching = false;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("Default", builder => {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
                options.Providers.Add<BrotliCompressionProvider>();
            });

            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            if (UseControllersWithResponseCaching)
            {
                services.AddResponseCaching();
                services.AddControllers();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("Default");
            app.UseResponseCompression();

            if (!UseControllersWithResponseCaching)
            {
                app.Map("/ip/location", x =>
                {
                    x.Run(context =>
                    {
                        string ip = context.Request.Query["ip"];
                        var result = LocationByIpFinder.Find(DbSnapshotHolder.Instance, ip);
                        context.Response.ContentType = "application/json";
                        return context.Response.WriteAsync(result);
                    });
                });

                app.Map("/city/locations", x =>
                {
                    x.Run(context =>
                    {
                        string city = context.Request.Query["city"];
                        var result = LocationsByCityFinder.Find(DbSnapshotHolder.Instance, city);
                        context.Response.ContentType = "application/json";
                        return context.Response.WriteAsync(result);
                    });
                });
            }

            if (UseControllersWithResponseCaching)
            {
                app.UseRouting();
                app.UseResponseCaching();

                app.Use(async (context, next) =>
                {
                    context.Response.GetTypedHeaders().CacheControl =
                        new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                        {
                            Public = true,
                            MaxAge = TimeSpan.FromSeconds(3600)
                        };
                    context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
                        new string[] { "Accept-Encoding" };

                    await next();
                });

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            }
        }
    }
}

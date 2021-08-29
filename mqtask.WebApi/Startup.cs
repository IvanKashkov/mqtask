using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO.Compression;
using mqtask.Application;
using Microsoft.AspNetCore.ResponseCompression;
using mqtask.Application.Queries;

namespace mqtask.WebApi
{
    public class Startup
    {
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("Default");
            app.UseResponseCompression();
            
            app.Map("/ip/location", x =>
            {
                x.Run(context =>
                {
                    string ip = context.Request.Query["ip"];
                    var result = LocationByIpFinder.Find(DbSnapshotHolder.Instance, ip);
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(result));
                });
            });

            app.Map("/city/locations", x =>
            {
                x.Run(context =>
                {
                    string city = context.Request.Query["city"];
                    var result = LocationsByCityFinder.Find(DbSnapshotHolder.Instance, city);
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(result));
                });
            });
        }
    }
}

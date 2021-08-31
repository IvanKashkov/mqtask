using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.IO.Compression;
using mqtask.Application;
using Microsoft.AspNetCore.ResponseCompression;
using mqtask.Application.Queries;
using mqtask.Domain.Entities;

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
                    context.Response.ContentType = "application/json";
                    Location location = LocationByIpFinder.Find(DbSnapshotHolder.Instance, ip);
                    var json = System.Text.Json.JsonSerializer.Serialize(location);
                    return context.Response.WriteAsync(json);
                });
            });

            app.Map("/city/locations", x =>
            {
                x.Run(context =>
                {
                    string city = context.Request.Query["city"];
                    context.Response.ContentType = "application/json";
                    var result = LocationsByCityFinder.Find(DbSnapshotHolder.Instance, city);
                    var json = System.Text.Json.JsonSerializer.Serialize(result);
                    return context.Response.WriteAsync(json);
                });
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Hal.Web.Api.Resources;
using AspNet.Hal.Web.Data;
using AspNet.Hal.Web.Models;
using DbUp;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Cors;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNet.Hal.Web
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            builder.AddEnvironmentVariables();
            var config = builder.Build();

            services.AddMvc(options =>
            {
                options.InputFormatters.Insert(0, new JsonHalInputFormatter());
                options.OutputFormatters.Insert(0, new JsonHalOutputFormatter());
                options.Filters.Add(new CorsAuthorizationFilterFactory("any"));
            });

            services.AddCors(options =>
            {
                // Define one or more CORS policies
                options.AddPolicy("any",
                    policy =>
                    {
                        // for HAL browser's purposes
                        policy.AllowCredentials().AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    });
            });

            services
                .AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<BeerDbContext>(options =>
                {
                    var connectionString = config["Data:DefaultConnection:ConnectionString"];
                    options.UseSqlServer(connectionString);

                    //var result = DeployChanges.To
                    //    .SqlDatabase(connectionString)
                    //    .WithScriptsFromFileSystem("Init.sql")
                    //    .Build()
                    //    .PerformUpgrade();
                });


            services.AddScoped<IRepository<BeerRepresentation>, BeerRepository<BeerRepresentation>>();
            services.AddScoped<IRepository<Beer>, BeerRepository<Beer>>();
            services.AddScoped<IBeerDbContext, BeerDbContext>();
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //this is the magic line
            loggerFactory.AddProvider(new MyLoggerProvider());

            app.UseIISPlatformHandler();

            app.UseMvc();

            
        }


        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }

    public class MyLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new MyLogger();
        }

        public void Dispose()
        { }

        private class MyLogger : ILogger
        {
            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Log(LogLevel logLevel, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
            {
                System.Diagnostics.Trace.WriteLine(formatter(state, exception));
            }

            public IDisposable BeginScopeImpl(object state)
            {
                return null;
            }
        }
    }
}

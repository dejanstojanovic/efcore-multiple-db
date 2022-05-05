using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MulitpleDb.Sample.Data;
using MulitpleDb.Sample.Swagger;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using FluentValidation.AspNetCore;
using FluentValidation;
using MulitpleDb.Sample.Validators;
using MulitpleDb.Sample.Models;
using Microsoft.Extensions.Logging;
using Serilog;
using MulitpleDb.Sample.Extensions;
using MulitpleDb.Sample.Constants;
using MulitpleDb.Sample.Options;
using MulitpleDb.Sample.Middlewares;

namespace MulitpleDb.Sample
{
    public class Startup
    {
        public IWebHostEnvironment HostingEnvironment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            HostingEnvironment = environment;

            Log.Logger = new LoggerConfiguration()
                 .Enrich.FromLogContext()
                 .ReadFrom.Configuration(configuration)
                 .CreateLogger();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(c => c.AddSerilog());
            services.AddScoped<GlobalListener>();
            services.AddScoped<GlobalCommandInterceptor>();

            services.AddDbContext<Database1Context>((provider, options) =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Database1"));
                DiagnosticListener.AllListeners.Subscribe(provider.GetRequiredService<GlobalListener>());
            });

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    })
                .AddFluentValidation();

            services.AddTransient<IValidator<RocketQueryModel>, RocketQueryModelValidator>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MulitpleDb.Sample", Version = "v1" });
                c.ParameterFilter<PlanetsParameterFilter>();

                c.AddApiKeyAuthSchemaSecurityDefinitions().AddBasicAuthSchemaSecurityDefinitions();

            }).AddSwaggerGenNewtonsoftSupport();


            services.Configure<AuthenticationConfig>(Configuration.GetSection(nameof(AuthenticationConfig)));
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = AuthenticationSchemaNames.ApiKeyAuthentication;
            })
            .AddApiKeyAuthenticationSchema()
            .AddBasicAuthenticationSchema();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Authenticated", policy => policy.RequireAuthenticatedUser());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MulitpleDb.Sample v1");
                });
            }


            app.RestricteStaticContent("/tent.jpg");
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

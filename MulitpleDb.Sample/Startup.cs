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

namespace MulitpleDb.Sample
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
            services.AddDbContext<Database1Context>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Database1"));
            });

            DiagnosticListener.AllListeners.Subscribe(new GlobalListener());

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

            }).AddSwaggerGenNewtonsoftSupport();
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

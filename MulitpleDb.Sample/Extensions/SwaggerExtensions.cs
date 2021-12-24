using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MulitpleDb.Sample.Constants;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MulitpleDb.Sample.Extensions
{
    public static class SwaggerExtensions
    {
        public static SwaggerGenOptions AddBasicAuthSchemaSecurityDefinitions(this SwaggerGenOptions options)
        {
            options.AddSecurityDefinition("basic", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "basic",
                In = ParameterLocation.Header,
                Description = "Basic Authorization header using the Bearer scheme."
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "basic"
                                }
                            },
                            new string[] {}
                    }
                });

            return options;
        }

        public static SwaggerGenOptions AddApiKeyAuthSchemaSecurityDefinitions(this SwaggerGenOptions options)
        {
            options.AddSecurityDefinition("token", new OpenApiSecurityScheme
            {
                Name = HeaderKeyNames.ApiKeyAuthenticationKey,
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Description = "Api key from header",
            });


            options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "token"
                                }
                            },
                            new string[] {}
                        }
                    });

            return options;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MulitpleDb.Sample.Constants;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

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
                Flows = new OpenApiOAuthFlows
                {
                    //Password = new OpenApiOAuthFlow
                    //{
                    //    AuthorizationUrl = new Uri($"{Configuration.GetValue<string>("Authentication:Authority")}/connect/authorize"),
                    //    Scopes = new Dictionary<string, string> { { "stack-api", "Stack Api access" } },
                    //    TokenUrl = new Uri($"{Configuration.GetValue<string>("Authentication:Authority")}/connect/token"),
                    //}

                    ClientCredentials = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"http://localhost:5000/api/authorize"),
                        TokenUrl = new Uri($"http://localhost:5000/api/authorize/token")
                    }
                }
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

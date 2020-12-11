using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;

namespace MulitpleDb.Sample.Swagger
{
    public class PlanetsParameterFilter : IParameterFilter
    {
        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            if (parameter.Name.Equals("planet"))
            {
                parameter.Schema.Items = new OpenApiSchema()
                {
                    Type = "array",
                    //Format = "string",
                    Enum = new List<IOpenApiAny>()
                        {
                            new OpenApiString("Item 1"),
                            new OpenApiString("Item 2")
                        },

                };
            }
        }
    }
}

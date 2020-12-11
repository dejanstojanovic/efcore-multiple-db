﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using MulitpleDb.Sample.Data;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MulitpleDb.Sample.Swagger
{
    public class PlanetsParameterFilter : IParameterFilter
    {
        readonly IServiceScopeFactory _serviceScopeFactory;

        public PlanetsParameterFilter(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            if (parameter.Name.Equals("planet"))
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var planetsContext = scope.ServiceProvider.GetRequiredService<Database1Context>();
                    IEnumerable<Planet> planets = planetsContext.Planets.ToArray();

                    parameter.Schema.Enum = planets.Select(p => new OpenApiString(p.Name)).ToList<IOpenApiAny>();

                }

                //parameter.Schema.Enum = new List<IOpenApiAny>()
                //        {
                //            new OpenApiString("Item 1"),
                //            new OpenApiString("Item 2")
                //        };
             }
        }
    }
}

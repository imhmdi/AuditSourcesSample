using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;

namespace AuditSourcesSample
{
    public class ClientVersionHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "client-name",
                In = ParameterLocation.Header,
                Required = true,
                Example= new OpenApiString("MyApp")
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "client-version",
                In = ParameterLocation.Header,
                Required = true,
                Example = new OpenApiString("1.0.17")
            });


        }

    }
}

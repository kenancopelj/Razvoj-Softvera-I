using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FIT_Api_Examples.Helper.AutentifikacijaAutorizacija
{
    public class AutorizacijaSwaggerHeader : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "autentifikacija-token",
                Description="Upisati autentifikacijski token",
                In=ParameterLocation.Header
            });
        }
    }
}

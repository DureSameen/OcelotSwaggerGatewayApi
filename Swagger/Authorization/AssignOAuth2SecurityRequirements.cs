using System.Collections.Generic;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Swagger.Authorization
{
    public class AssignOAuth2SecurityRequirements : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {

            if (operation.Security == null)
                operation.Security = new List<IDictionary<string, IEnumerable<string>>>();

            var oAuthRequirements = new Dictionary<string, IEnumerable<string>>
        {
            {"oauth2", new List<string> {"sampleapi"}}
        };

            operation.Security.Add(oAuthRequirements);
        }
    }
}

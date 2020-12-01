using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace AuditSourcesSample
{
    public static class JsonDbFunctions
    {
        public static string Value(
            string expression,
            string path)
        {
            // for UseInMemoryDatabase provider support

            var dynamicObject = JsonSerializer.Deserialize<ExpandoObject>(expression);

            var jsonFieldName = path.Replace("$.", "");

            return dynamicObject.FirstOrDefault(p => p.Key == jsonFieldName).Value.ToString();

            //throw new InvalidOperationException($"{nameof(Value)}cannot be called client side");
        }
    }

    public static class ModelBuilderExtensions
    {
        public static ModelBuilder UseJsonDbFunctions(this ModelBuilder builder)
        {
            var jsonvalueMethodInfo = typeof(JsonDbFunctions)
                .GetRuntimeMethod(
                    nameof(JsonDbFunctions.Value),
                    new[] { typeof(string), typeof(string) }
                );
            builder
            .HasDbFunction(jsonvalueMethodInfo)
            .HasTranslation(args => new SqlFunctionExpression(
                "JSON_VALUE",
                args,
                true,
                args.Select(x => true),
                typeof(string),
                null)
            );
            return builder;
        }
    }
}
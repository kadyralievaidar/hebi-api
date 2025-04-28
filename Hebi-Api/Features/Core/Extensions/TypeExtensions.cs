using System.Reflection;

namespace Hebi_Api.Features.Core.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsOpenGeneric(this Type type)
        {
            if (!type.GetTypeInfo().IsGenericTypeDefinition)
                return type.GetTypeInfo().ContainsGenericParameters;

            return true;
        }
    }
}

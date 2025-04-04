using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Hebi_Api.Tests;
public static class TestHelper
{
    public static IMapper CreateMapper(Type startUpProfileAssemblyType)
    {
        ServiceCollection services = new ();
        services.AddAutoMapper(startUpProfileAssemblyType);
        return (IMapper)services.BuildServiceProvider().GetService(typeof(IMapper));
    }
}

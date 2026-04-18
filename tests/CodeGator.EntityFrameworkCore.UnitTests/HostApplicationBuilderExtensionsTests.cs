using CodeGator.EntityFrameworkCore.Interceptors.Audit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodeGator.EntityFrameworkCore.UnitTests;

[TestClass]
public sealed class HostApplicationBuilderExtensionsTests
{
    [TestMethod]
    public void AddCodeGatorAuditInterceptors_registers_interceptors_and_http_context_accessor()
    {
        var builder = Host.CreateApplicationBuilder();

        builder.AddCodeGatorAuditInterceptors();

        using var host = builder.Build();
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        Assert.IsInstanceOfType(
            services.GetRequiredService<IHttpContextAccessor>(),
            typeof(HttpContextAccessor)
            );

        _ = services.GetRequiredService<AuditedEntityInterceptor>();
        _ = services.GetRequiredService<ImmutableAuditedEntityInterceptor>();
    }

    [TestMethod]
    public void AddCodeGatorAuditInterceptors_returns_same_builder_instance()
    {
        var builder = Host.CreateApplicationBuilder();

        var returned = builder.AddCodeGatorAuditInterceptors();

        Assert.AreSame(builder, returned);
    }
}

using System.Security.Claims;
using CodeGator.EntityFrameworkCore.Interceptors.Audit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CodeGator.EntityFrameworkCore.UnitTests.Support;

internal static class AuditTestDbContextFactory
{
    public static AuditTestDbContext CreateAudited(
        IHttpContextAccessor httpAccessor,
        ILogger<AuditedEntityInterceptor>? logger = null
        )
    {
        var interceptor = new AuditedEntityInterceptor(
            httpAccessor,
            logger ?? NullLogger<AuditedEntityInterceptor>.Instance
            );

        var options = new DbContextOptionsBuilder<AuditTestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .AddInterceptors(interceptor)
            .Options;

        return new AuditTestDbContext(options);
    }

    public static AuditTestDbContext CreateImmutable(
        IHttpContextAccessor httpAccessor,
        ILogger<ImmutableAuditedEntityInterceptor>? logger = null
        )
    {
        var interceptor = new ImmutableAuditedEntityInterceptor(
            httpAccessor,
            logger ?? NullLogger<ImmutableAuditedEntityInterceptor>.Instance
            );

        var options = new DbContextOptionsBuilder<AuditTestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .AddInterceptors(interceptor)
            .Options;

        return new AuditTestDbContext(options);
    }

    public static HttpContextAccessor HttpAccessorWithoutUser()
    {
        return new HttpContextAccessor { HttpContext = null };
    }

    public static HttpContextAccessor HttpAccessorWithNameIdentifier(string value)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.User = new ClaimsPrincipal(
            new ClaimsIdentity(
                [new Claim(ClaimTypes.NameIdentifier, value)],
                authenticationType: "test"
                )
            );

        return new HttpContextAccessor { HttpContext = httpContext };
    }
}

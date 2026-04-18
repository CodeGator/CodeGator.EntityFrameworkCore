using CodeGator.EntityFrameworkCore.Interceptors.Audit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;

namespace CodeGator.EntityFrameworkCore.UnitTests;

[TestClass]
public sealed class InterceptorConstructorTests
{
    [TestMethod]
    public void AuditedEntityInterceptor_throws_when_http_accessor_is_null()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() =>
            _ = new AuditedEntityInterceptor(
                null!,
                NullLogger<AuditedEntityInterceptor>.Instance
                )
            );
    }

    [TestMethod]
    public void AuditedEntityInterceptor_throws_when_logger_is_null()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() =>
            _ = new AuditedEntityInterceptor(
                new HttpContextAccessor(),
                null!
                )
            );
    }

    [TestMethod]
    public void ImmutableAuditedEntityInterceptor_throws_when_http_accessor_is_null()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() =>
            _ = new ImmutableAuditedEntityInterceptor(
                null!,
                NullLogger<ImmutableAuditedEntityInterceptor>.Instance
                )
            );
    }

    [TestMethod]
    public void ImmutableAuditedEntityInterceptor_throws_when_logger_is_null()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() =>
            _ = new ImmutableAuditedEntityInterceptor(
                new HttpContextAccessor(),
                null!
                )
            );
    }
}

using CodeGator.EntityFrameworkCore.UnitTests.Support;

namespace CodeGator.EntityFrameworkCore.UnitTests;

[TestClass]
public sealed class ImmutableAuditedEntityInterceptorTests
{
    [TestMethod]
    public async Task SaveChangesAsync_on_added_entity_sets_creation_audit_only()
    {
        await using var db = AuditTestDbContextFactory.CreateImmutable(
            AuditTestDbContextFactory.HttpAccessorWithNameIdentifier("immutable-user")
            );

        var before = DateTimeOffset.UtcNow;
        var entity = new SampleImmutableEntity { Name = "row" };
        db.ImmutableSamples.Add(entity);
        await db.SaveChangesAsync();
        var after = DateTimeOffset.UtcNow;

        Assert.AreEqual("IMMUTABLE-USER", entity.CreatedBy);
        Assert.IsTrue(entity.CreatedAt >= before && entity.CreatedAt <= after);
    }

    [TestMethod]
    public void SaveChanges_on_added_entity_sets_creation_audit_using_os_user_when_no_http_user()
    {
        using var db = AuditTestDbContextFactory.CreateImmutable(
            AuditTestDbContextFactory.HttpAccessorWithoutUser()
            );

        var before = DateTimeOffset.UtcNow;
        var entity = new SampleImmutableEntity { Name = "sync" };
        db.ImmutableSamples.Add(entity);
        db.SaveChanges();
        var after = DateTimeOffset.UtcNow;

        Assert.AreEqual(Environment.UserName, entity.CreatedBy);
        Assert.IsTrue(entity.CreatedAt >= before && entity.CreatedAt <= after);
    }
}

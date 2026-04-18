using CodeGator.EntityFrameworkCore.UnitTests.Support;

namespace CodeGator.EntityFrameworkCore.UnitTests;

[TestClass]
public sealed class AuditedEntityInterceptorTests
{
    [TestMethod]
    public async Task SaveChangesAsync_on_added_entity_sets_created_audit_using_os_user_when_no_http_user()
    {
        await using var db = AuditTestDbContextFactory.CreateAudited(
            AuditTestDbContextFactory.HttpAccessorWithoutUser()
            );

        var before = DateTimeOffset.UtcNow;
        var entity = new SampleAuditedEntity { Name = "one" };
        db.AuditedSamples.Add(entity);
        await db.SaveChangesAsync();
        var after = DateTimeOffset.UtcNow;

        Assert.AreEqual(Environment.UserName, entity.CreatedBy);
        Assert.IsTrue(entity.CreatedAt >= before && entity.CreatedAt <= after);
        Assert.IsNull(entity.UpdatedAt);
        Assert.IsNull(entity.UpdatedBy);
    }

    [TestMethod]
    public void SaveChanges_on_added_entity_sets_created_audit_from_name_identifier_claim()
    {
        using var db = AuditTestDbContextFactory.CreateAudited(
            AuditTestDbContextFactory.HttpAccessorWithNameIdentifier("user-abc")
            );

        var before = DateTimeOffset.UtcNow;
        var entity = new SampleAuditedEntity { Name = "two" };
        db.AuditedSamples.Add(entity);
        db.SaveChanges();
        var after = DateTimeOffset.UtcNow;

        Assert.AreEqual("USER-ABC", entity.CreatedBy);
        Assert.IsTrue(entity.CreatedAt >= before && entity.CreatedAt <= after);
    }

    [TestMethod]
    public async Task SaveChangesAsync_on_modified_entity_sets_updated_audit()
    {
        await using var db = AuditTestDbContextFactory.CreateAudited(
            AuditTestDbContextFactory.HttpAccessorWithNameIdentifier("editor-1")
            );

        var entity = new SampleAuditedEntity { Name = "orig" };
        db.AuditedSamples.Add(entity);
        await db.SaveChangesAsync();

        entity.Name = "updated";
        var beforeUpdate = DateTimeOffset.UtcNow;
        await db.SaveChangesAsync();
        var afterUpdate = DateTimeOffset.UtcNow;

        Assert.IsNotNull(entity.UpdatedAt);
        Assert.IsTrue(entity.UpdatedAt >= beforeUpdate && entity.UpdatedAt <= afterUpdate);
        Assert.AreEqual("EDITOR-1", entity.UpdatedBy);
    }

    [TestMethod]
    public async Task SaveChangesAsync_with_no_changes_does_not_throw()
    {
        await using var db = AuditTestDbContextFactory.CreateAudited(
            AuditTestDbContextFactory.HttpAccessorWithoutUser()
            );

        await db.SaveChangesAsync();
    }
}

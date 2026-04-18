using CodeGator.EntityFrameworkCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodeGator.EntityFrameworkCore.UnitTests.Support;

public sealed class SampleAuditedEntity : AuditedEntityBase
{
    public int Id { get; set; }

    public string Name { get; set; } = "";
}

public sealed class SampleImmutableEntity : ImmutableAuditedEntityBase
{
    public int Id { get; set; }

    public string Name { get; set; } = "";
}

public sealed class AuditTestDbContext : DbContext
{
    public AuditTestDbContext(DbContextOptions<AuditTestDbContext> options)
        : base(options)
    {
    }

    public DbSet<SampleAuditedEntity> AuditedSamples => Set<SampleAuditedEntity>();

    public DbSet<SampleImmutableEntity> ImmutableSamples => Set<SampleImmutableEntity>();
}

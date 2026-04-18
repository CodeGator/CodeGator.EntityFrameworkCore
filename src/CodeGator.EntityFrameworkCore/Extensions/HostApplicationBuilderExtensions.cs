#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.Extensions.Hosting;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// This class provides extension methods for configuring IHostApplicationBuilder.
/// </summary>
public static partial class HostApplicationBuilderExtensions
{
    /// <summary>
    /// This method registers CodeGator EF Core audit interceptor services.
    /// </summary>
    /// <typeparam name="T">Concrete host application builder type.</typeparam>
    /// <param name="builder">The host application builder receiving services.</param>
    /// <param name="bootstrapLogger">Optional logger for bootstrap diagnostics.</param>
    /// <returns>The same builder instance for call chaining.</returns>
    /// <remarks>
    /// <para>
    /// Audit interceptors automatically populate the <c>CreatedAt</c>, 
    /// <c>CreatedBy</c>, <c>UpdatedAt</c>, and <c>UpdatedBy</c> properties
    /// before an entity derived from <see cref="AuditedEntityBase"/> or 
    /// <see cref="ImmutableAuditedEntityBase"/> are written to the database.
    /// </para>
    /// </remarks>
    public static T AddCodeGatorAuditInterceptors<T>(
        [NotNull] this T builder,
        [AllowNull] ILogger? bootstrapLogger = null
        ) where T : IHostApplicationBuilder
    {
        if (true == bootstrapLogger?.IsEnabled(LogLevel.Debug))
        {
            bootstrapLogger?.LogDebug(
                "Adding audit interceptors"
                );
        }

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSingleton<AuditedEntityInterceptor>();
        builder.Services.AddSingleton<ImmutableAuditedEntityInterceptor>();

        return builder;
    }
}

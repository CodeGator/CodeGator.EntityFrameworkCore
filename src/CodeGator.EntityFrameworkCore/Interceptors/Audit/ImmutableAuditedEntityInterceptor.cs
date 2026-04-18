namespace CodeGator.EntityFrameworkCore.Interceptors.Audit;

/// <summary>
/// This class applies creation audit metadata for ImmutableAuditedEntityBase types.
/// </summary>
/// <remarks>
/// <para>
/// Automatically supplies <see cref="IImmutableAuditedEntity.CreatedAt"/> and
/// <see cref="IImmutableAuditedEntity.CreatedBy"/> when immutable audited entities are inserted.
/// </para>
/// </remarks>
public sealed partial class ImmutableAuditedEntityInterceptor : SaveChangesInterceptor
{
    /// <summary>
    /// This field contains the HTTP context accessor for this class.
    /// </summary>
    private readonly IHttpContextAccessor _httpContextAccesor;

    /// <summary>
    /// This field contains the logger for this class.
    /// </summary>
    private readonly ILogger<ImmutableAuditedEntityInterceptor> _logger;

    /// <summary>
    /// This constructor initializes a new instance of the ImmutableAuditedEntityInterceptor class.
    /// </summary>
    /// <param name="httpContextAccesor">HTTP context accessor resolved from ASP.NET Core DI.</param>
    /// <param name="logger">Logger for diagnostic messages from this interceptor.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="httpContextAccesor"/> or <paramref name="logger"/> is null.</exception>
    public ImmutableAuditedEntityInterceptor(
        [NotNull] IHttpContextAccessor httpContextAccesor,
        [NotNull] ILogger<ImmutableAuditedEntityInterceptor> logger
        )
    {
        Guard.Instance().ThrowIfNull(httpContextAccesor, nameof(httpContextAccesor))
            .ThrowIfNull(logger, nameof(logger));

        _httpContextAccesor = httpContextAccesor;
        _logger = logger;
    }

    /// <summary>
    /// This method stamps immutable entities before a synchronous save operation.
    /// </summary>
    /// <param name="eventData">Diagnostic data for the active save operation.</param>
    /// <param name="result">The interception result from earlier interceptors.</param>
    /// <returns>The interception result passed to the remainder of the pipeline.</returns>
    public override InterceptionResult<int> SavingChanges(
        [NotNull] DbContextEventData eventData,
        InterceptionResult<int> result
        )
    {
        BeforeSaveTriggers(eventData.Context);
        return result;
    }

    /// <summary>
    /// This method stamps immutable entities before an asynchronous save operation.
    /// </summary>
    /// <param name="eventData">Diagnostic data for the active save operation.</param>
    /// <param name="result">The interception result from earlier interceptors.</param>
    /// <param name="cancellationToken">Token monitored while the interceptor runs.</param>
    /// <returns>A value task returning the interception result for the pipeline.</returns>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
        )
    {
        BeforeSaveTriggers(eventData?.Context);
        return ValueTask.FromResult(result);
    }

    // Private methods.

    /// <summary>
    /// This method stamps creation audit fields on new immutable audited entities.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Walks added entries and, for each <see cref="IImmutableAuditedEntity"/>, sets creation fields.
    /// The current user is taken from the name identifier claim when present, otherwise from
    /// <see cref="Environment.UserName"/>.
    /// </para>
    /// </remarks>
    /// <param name="context">The DbContext whose change tracker is evaluated.</param>
    private void BeforeSaveTriggers(
        [AllowNull] DbContext? context
        )
    {
        ZBeforeSaveTriggersEntry(_logger);

        var entries = context?.ChangeTracker.Entries().Where(
            x => x.State == EntityState.Added
            ).ToList();

        if ((entries?.Count ?? 0) == 0)
        {
            ZNoChanges(_logger);
            return;
        }

        var currentClaimsPrincipal = _httpContextAccesor.HttpContext?.User;

        var nameIdClaim = currentClaimsPrincipal?.Claims.FirstOrDefault(x =>
            x.Type == ClaimTypes.NameIdentifier
            );

        // The "current" user is either:
        //   (A) the currently authenticated user, or
        //   (B) the current OS user.

        var currentUser = nameIdClaim?.Value.ToUpper()
            ?? Environment.UserName;

        foreach (var entityEntry in entries ?? [])
        {
            if (entityEntry.Entity is IImmutableAuditedEntity auditedEntity)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    auditedEntity.CreatedAt = DateTimeOffset.UtcNow;
                    auditedEntity.CreatedBy = currentUser;
                }
            }
        }
    }

    // Log methods.

    /// <summary>
    /// This method emits a debug log when auditing begins for a save operation.
    /// </summary>
    /// <param name="logger">The logger receiving the diagnostic entry.</param>
    [LoggerMessage(
        EventId = 1000,
        Level = LogLevel.Debug,
        Message = "Entering BeforeSaveTriggers")]
    private static partial void ZBeforeSaveTriggersEntry(ILogger logger);

    /// <summary>
    /// This method emits a debug log when no tracked changes require auditing.
    /// </summary>
    /// <param name="logger">The logger receiving the diagnostic entry.</param>
    [LoggerMessage(
        EventId = 1001,
        Level = LogLevel.Debug,
        Message = "No changes detected")]
    private static partial void ZNoChanges(ILogger logger);
}

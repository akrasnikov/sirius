#nullable enable
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProjectName.Core.Abstractions.Entities;
using ProjectName.Core.Abstractions.Extensions;
using ProjectName.Core.Abstractions.Interfaces;

namespace ProjectName.Core.Abstractions.Context;

public abstract class BaseDbContext : DbContext
{
    private readonly ITenantResolver  _tenantResolver;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    protected BaseDbContext(DbContextOptions<BaseDbContext> options, ITenantResolver tenantResolver,
        IHttpContextAccessor? httpContextAccessor = null) : base(options)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _tenantResolver = tenantResolver ?? throw new ArgumentNullException(nameof(tenantResolver));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        DefaultSchemaFactory(modelBuilder, null);
        EntityConfigurationFactory(modelBuilder);
        ConfigureContext(modelBuilder);
        
        modelBuilder.AppendGlobalQueryFilter<ISoftDelete>(s => s.DeletedOn == null);
    }

    protected virtual void DefaultSchemaFactory(ModelBuilder modelBuilder, string? schema)
    {
        modelBuilder.HasDefaultSchema(schema ?? throw new ArgumentNullException($"" +
            $"the schema for the database is not specified, it is necessary to override DefaultSchemaFactory "));
    }
    protected virtual void EntityConfigurationFactory(ModelBuilder modelBuilder)
    {
         // modelBuilder.ApplyConfiguration(new ContractorConfiguration());
    }
    private static void ConfigureContext(ModelBuilder builder)
    {
        foreach (var eType in builder.Model.GetEntityTypes())
        {
            (eType.FindProperty(nameof(IAuditableEntity.CreatedOn)) ?? 
             throw new InvalidOperationException($"{nameof(eType)} - not find property {nameof(IAuditableEntity.CreatedOn)}"))
                .SetDefaultValueSql("now() at time zone 'utc'");
            
            (eType.FindProperty(nameof(ISoftDelete.IsDeleted)) ?? 
             throw new InvalidOperationException($"{nameof(eType)} - not find property {nameof(ISoftDelete.IsDeleted)}"))
                .SetDefaultValueSql("false");
        }
    }
    public override int SaveChanges()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is IAuditableEntity && e.State is EntityState.Added or EntityState.Modified);
        foreach (var entityEntry in entries)
        {
            ((IAuditableEntity)entityEntry.Entity).LastModifiedOn = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added && entityEntry.Entity is IEntity<Guid> entity && entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }
        }
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var userId =  _httpContextAccessor?.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) 
                      ?? throw new ArgumentNullException($"userId not find when save dbContext");
        var tenantId = _tenantResolver.GetCurrentTenant().Name;
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                {
                    entry.Entity.CreatedBy = Guid.Parse(userId); 
                    entry.Entity.TenantId = tenantId;
                    if (entry.Entity is IEntity<Guid> entity && entity.Id == Guid.Empty) entity.Id = Guid.NewGuid();
                    break;
                }

                case EntityState.Modified:
                    entry.Entity.LastModifiedOn = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy = Guid.Parse(userId);
                    break;

                case EntityState.Deleted:
                    if (entry.Entity is ISoftDelete softDelete)
                    {
                        softDelete.DeletedBy = Guid.Parse(userId);
                        softDelete.DeletedOn = DateTime.UtcNow;
                        entry.State = EntityState.Modified;
                    }

                    break;
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                default:
                    break;
            }
        }
        return await base.SaveChangesAsync(true, cancellationToken);
    }

    // private IDbContextTransaction _currentTransaction;
    //
    // public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    // {
    //     if (_currentTransaction != null) return null;
    //
    //     _currentTransaction = await Database.BeginTransactionAsync(cancellationToken);
    //
    //     return _currentTransaction;
    // }
    //
    // public async Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
    // {
    //     if (transaction == null) throw new ArgumentNullException(nameof(transaction));
    //     if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");
    //
    //     try
    //     {
    //         await SaveChangesAsync(cancellationToken);
    //         transaction.Commit();
    //     }
    //     catch
    //     {
    //         RollbackTransaction();
    //         throw;
    //     }
    //     finally
    //     {
    //         if (_currentTransaction != null)
    //         {
    //             _currentTransaction.Dispose();
    //             _currentTransaction = null;
    //         }
    //     }
    // }
    //
    // public void RollbackTransaction()
    // {
    //     try
    //     {
    //         _currentTransaction?.Rollback();
    //     }
    //     finally
    //     {
    //         if (_currentTransaction != null)
    //         {
    //             _currentTransaction.Dispose();
    //             _currentTransaction = null;
    //         }
    //     }
    // }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectName.Auth.Domain.Common.Interfaces;
using ProjectName.Auth.Domain.Entities.Identity;

namespace ProjectName.Auth.Infrastructure.Contexts
{
    public class AuthenticationDBContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public AuthenticationDBContext(DbContextOptions<AuthenticationDBContext> options) : base(options) { }
       
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("Identity");
            ConfigureContext(builder);
            ConfigureTableNames(builder);
            //builder.Entity<User>(entity =>
            //{
            //    entity.ToTable(name: "User");
            //});
            //builder.Entity<IdentityRole>(entity =>
            //{
            //    entity.ToTable(name: "Role");
            //});
            //builder.Entity<IdentityUserRole<string>>(entity =>
            //{
            //    entity.ToTable("UserRoles");
            //});

            //builder.Entity<IdentityUserClaim<string>>(entity =>
            //{
            //    entity.ToTable("UserClaims");
            //});

            //builder.Entity<IdentityUserLogin<string>>(entity =>
            //{
            //    entity.ToTable("UserLogins");
            //});

            //builder.Entity<IdentityRoleClaim<string>>(entity =>
            //{
            //    entity.ToTable("RoleClaims");

            //});

            //builder.Entity<IdentityUserToken<string>>(entity =>
            //{
            //    entity.ToTable("UserTokens");
            //});
        }
        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is IEntityBase && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((IEntityBase)entityEntry.Entity).ModifiedDate = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added && entityEntry.Entity is IEntity<Guid> entity &&
                    entity.Id == Guid.Empty)
                {
                    entity.Id = Guid.NewGuid();
                }
            }

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is IEntityBase && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((IEntityBase)entityEntry.Entity).ModifiedDate = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added && entityEntry.Entity is IEntity<Guid> entity &&
                    entity.Id == Guid.Empty)
                {
                    entity.Id = Guid.NewGuid();
                }
            }

            return await base.SaveChangesAsync(true, cancellationToken);
        }

        private void ConfigureContext(ModelBuilder builder)
        { 
            foreach (var eType in builder.Model.GetEntityTypes())
            {
                if (!typeof(IEntityBase).IsAssignableFrom(eType.ClrType)) continue;
                eType.FindProperty(nameof(IEntityBase.CreatedDate))
                    .SetDefaultValueSql("now() at time zone 'utc'");
                eType.FindProperty(nameof(IEntityBase.ModifiedDate))
                    .SetDefaultValueSql("now() at time zone 'utc'");
                eType.FindProperty(nameof(IEntityBase.IsDeleted)).SetDefaultValueSql("false");
            }
        }

        private void ConfigureTableNames(ModelBuilder builder)
        {
            builder.Entity<Role>().ToTable("roles");
            builder.Entity<RoleClaim>().ToTable("role_claims");
            builder.Entity<UserRole>().ToTable("users_to_roles");
            builder.Entity<User>().ToTable("users");
            builder.Entity<UserLogin>().ToTable("user_logins");
            builder.Entity<UserClaim>().ToTable("user_claims");
            builder.Entity<UserToken>().ToTable("user_tokens");
        }

    }
}

using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectName.Core.Abstractions.Configurations;
using ProjectName.Core.Abstractions.Context;
using ProjectName.Core.Abstractions.Enums;

namespace ProjectName.Core.Abstractions.Extensions;

public static class DatabaseExtensions
{
    public static void RegisterDbContext<TBaseDbContext>(this IServiceCollection services,
        IConfiguration configuration, string connectionString)
        where TBaseDbContext : BaseDbContext
    {
        var dbProvider = configuration.GetSection(nameof(DbProviderConfiguration)).Get<DbProviderConfiguration>();

        var evolutionJuniorDbConnection = configuration.GetConnectionString(connectionString);

        switch (dbProvider.ProviderType)
        {
            case DbProviderType.PostgreSql:
                services.RegisterNpgSqlDbContexts<TBaseDbContext>(evolutionJuniorDbConnection);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dbProvider.ProviderType),
                    $@"The value needs to be one of {string.Join(", ", Enum.GetNames(typeof(DbProviderType)))}.");
        }
    }

    private static void RegisterNpgSqlDbContexts<TBaseDbContext>(
        this IServiceCollection services, string connectionString)
        where TBaseDbContext : BaseDbContext
    {
        var migrationsAssembly = typeof(DatabaseExtensions).GetTypeInfo().Assembly.GetName().Name;

        services.AddDbContext<TBaseDbContext>(options =>
        {
            options.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
            // options.UseSnakeCaseNamingConvention();
            options.ConfigureWarnings(cw => cw.Ignore(RelationalEventId.BoolWithDefaultWarning));
        });
    }
}
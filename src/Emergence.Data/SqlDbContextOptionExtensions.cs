using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Emergence.Data
{
    public static class SqlDbContextOptionExtensions
    {
        public static SqlServerDbContextOptionsBuilder AddConnectionResiliency(this SqlServerDbContextOptionsBuilder options, string migrationsAssembly)
        {
            options.MigrationsAssembly(migrationsAssembly);
            options.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
            return options;
        }

        public static DbContextOptionsBuilder AddSqlConnection(this DbContextOptionsBuilder options, string connectionString, string migrationsAssembly)
        {
            options.UseSqlServer(connectionString, sqlServerOptions =>
            {
                sqlServerOptions.AddConnectionResiliency(migrationsAssembly);
            });
            return options;
        }
    }
}

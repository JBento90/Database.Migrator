﻿using Microsoft.Extensions.Logging;

namespace SimpleSoft.Database.Migrator.Tests.Relational.SqlServer.Migrations
{
    public class ApplyMigrationsContext : SqlServerMigrationContext<SqlServerContextOptions<ApplyMigrationsContext>>
    {
        /// <inheritdoc />
        public ApplyMigrationsContext(
            SqlServerContextOptions<ApplyMigrationsContext> options, ILogger<ApplyMigrationsContext> logger)
            : base(options, logger)
        {

        }
    }
}
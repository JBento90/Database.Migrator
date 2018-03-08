﻿using System;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Database.Migrator.Tests
{
    public static class LoggingManager
    {
        public static readonly IMigrationLoggerFactory LoggerFactory;

        static LoggingManager()
        {
            LoggerFactory = new MigrationLoggerFactory(
                new LoggerFactory()
                    .AddConsole(LogLevel.Trace, true)
                    .AddDebug(LogLevel.Trace));
        }
    }
}

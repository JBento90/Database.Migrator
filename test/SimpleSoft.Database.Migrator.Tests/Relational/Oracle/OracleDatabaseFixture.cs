﻿using System;
using System.Data.Common;
using Dapper;
using Oracle.ManagedDataAccess.Client;

namespace SimpleSoft.Database.Migrator.Tests.Relational.Oracle
{
    public abstract class OracleDatabaseFixture : IDisposable
    {
        private readonly string _userName;
        private readonly string _userPassword;
        private readonly bool _dropUser;

        protected OracleDatabaseFixture(
            string masterConnectionString, string connectionString, 
            string userName, string userPassword, bool dropUser = false)
        {
            _userName = userName.ToUpperInvariant();
            _userPassword = userPassword;
            _dropUser = dropUser;

            MasterConnectionString = masterConnectionString;
            MasterConnection = new OracleConnection(MasterConnectionString);

            PrepareDatabase();

            ConnectionString = connectionString;
            Connection = new OracleConnection(ConnectionString);
        }

        ~OracleDatabaseFixture()
        {
            Dispose(false);
        }

        public string MasterConnectionString { get; }

        public DbConnection MasterConnection { get; private set; }

        public string ConnectionString { get; }

        public DbConnection Connection { get; private set; }


        #region Implementation of IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                MasterConnection?.Dispose();
                Connection?.Dispose();
            }

            MasterConnection = null;
            Connection = null;
        }

        #endregion

        private void PrepareDatabase()
        {
            MasterConnection.Open();
            try
            {
                var timeout = MasterConnection.ConnectionTimeout;

                var userCount = MasterConnection.QuerySingle<int>(@"
SELECT 
    COUNT(*) USER_COUNT
FROM SYS.DBA_USERS
WHERE
    USERNAME = :UserName", new
                {
                    UserName = _userName
                }, null, timeout);
                if (userCount > 0)
                {
                    if (_dropUser)
                        MasterConnection.Execute($"DROP USER {_userName} CASCADE", null, null, timeout);
                    else
                        return;
                }
                MasterConnection.Execute($@"CREATE USER {_userName} IDENTIFIED BY {_userPassword}", null, null, timeout);
                MasterConnection.Execute($@"GRANT DBA TO {_userName}", null, null, timeout);
            }
            finally
            {
                MasterConnection.Close();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DbSetup
{
    public static class ConnectionString
    {
        private static readonly string _connectionStringTemplate = "Server={0};Port={1};Database={2};User Id={3};Password={4};";
        private static readonly string _serverKey = "MBNM_DB_SERVER";
        private static readonly string _portKey = "MBNM_DB_PORT";
        private static readonly string _databaseKey = "MBNM_DB_DATABASE";
        private static readonly string _userKey = "MBNM_DB_USER";
        private static readonly string _passwordKey = "MBNM_DB_PASSWORD";
        public static string GetConnectionString()
        {
            var server = Environment.GetEnvironmentVariable(_serverKey) ?? throw new InvalidOperationException($"Environment variable '{_serverKey}' is not set.");
            var port = Environment.GetEnvironmentVariable(_portKey) ?? throw new InvalidOperationException($"Environment variable '{_portKey}' is not set.");
            var database = Environment.GetEnvironmentVariable(_databaseKey) ?? throw new InvalidOperationException($"Environment variable '{_databaseKey}' is not set.");
            var user = Environment.GetEnvironmentVariable(_userKey) ?? throw new InvalidOperationException($"Environment variable '{_userKey}' is not set.");
            var password = Environment.GetEnvironmentVariable(_passwordKey) ?? throw new InvalidOperationException($"Environment variable '{_passwordKey}' is not set.");

            return string.Format(_connectionStringTemplate, server, port, database, user, password);
        }
    }
}

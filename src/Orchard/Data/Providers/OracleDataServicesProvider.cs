using System;
using FluentNHibernate.Cfg.Db;

namespace Orchard.Data.Providers
{
    public class OracleDataServicesProvider : AbstractDataServicesProvider
    {
        private readonly string _dataFolder;
        private readonly string _connectionString;

        public OracleDataServicesProvider(string dataFolder, string connectionString)
        {
            _dataFolder = dataFolder;
            _connectionString = connectionString;
        }

        public static string ProviderName
        {
            get { return "Oracle"; }
        }

        public override IPersistenceConfigurer GetPersistenceConfigurer(bool createDatabase)
        {
            var persistence = OracleManagedDataClientConfiguration.Oracle10;
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new ArgumentException("The connection string is empty");
            }
            persistence = persistence.ConnectionString(_connectionString);
            return persistence;
        }

        public static string GetAlias(string name)
        {
            return name.Length > 30 ? name.Substring(0, 30).ToUpper() : name.ToUpper();
        }

        public static int GetColumnLength(int length)
        {
            return length > 2000 ? 2000 : length;
        }
    }
}
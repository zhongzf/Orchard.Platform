using System;
using System.IO;
using FluentNHibernate.Cfg.Db;
using Orchard.Data.Providers;
using Orchard.Environment.Extensions;

namespace Orchard.Data.Providers
{
    public class SQLiteDataServicesProvider : AbstractDataServicesProvider
    {
        private readonly string _fileName;
        private readonly string _dataFolder;
        private readonly string _connectionString;

        public SQLiteDataServicesProvider(string dataFolder, string connectionString)
        {
            _dataFolder = dataFolder;
            _connectionString = connectionString;

            _dataFolder = dataFolder;
            _connectionString = connectionString;
            _fileName = Path.Combine(_dataFolder, "Orchard.db");
        }

        public static string ProviderName
        {
            get { return "SQLite"; }
        }

        public override IPersistenceConfigurer GetPersistenceConfigurer(bool createDatabase)
        {
            var persistence = CsharpSqliteConfiguration.Standard;
            if (string.IsNullOrEmpty(_connectionString))
            {
                if (!Directory.Exists(_dataFolder))
                {
                    Directory.CreateDirectory(_dataFolder);
                }

                if (createDatabase && File.Exists(_fileName))
                {
                    File.Delete(_fileName);
                }

                persistence = persistence.UsingFile(_fileName);
            }
            else
            {
                persistence = persistence.ConnectionString(_connectionString);
            }

            return persistence;
        }
    }
}
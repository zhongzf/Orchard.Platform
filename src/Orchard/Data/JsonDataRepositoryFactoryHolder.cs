using Orchard.Environment;
using Orchard.Environment.Configuration;
using Orchard.Environment.ShellBuilders.Models;
using Orchard.Localization;
using Orchard.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orchard.Data
{
    public interface IJsonDataRepositoryFactoryHolder : ISingletonDependency
    {
        IJsonDataRepositoryFactory GetRepositoryFactory();
    }

    public class JsonDataRepositoryFactoryHolder : IJsonDataRepositoryFactoryHolder
    {
        private static readonly ConcurrentDictionary<string, IJsonDataRepositoryFactory> factories = new ConcurrentDictionary<string, IJsonDataRepositoryFactory>();

        private readonly ShellSettings _shellSettings;

        public JsonDataRepositoryFactoryHolder(
            ShellSettings shellSettings)
        {
            _shellSettings = shellSettings;

            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public Localizer T { get; set; }
        public ILogger Logger { get; set; }


        private IJsonDataRepositoryFactory _jsonDataRepositoryFactory;
        public IJsonDataRepositoryFactory GetRepositoryFactory()
        {
            if (_jsonDataRepositoryFactory == null)
            {
                lock (this)
                {
                    if (_jsonDataRepositoryFactory == null)
                    {
                        _jsonDataRepositoryFactory = factories.GetOrAdd(_shellSettings.Name, _ => { return BuildRepositoryFactory(); });
                    }
                }
            }
            return _jsonDataRepositoryFactory;
        }

        private IJsonDataRepositoryFactory BuildRepositoryFactory()
        {
            Logger.Debug("Building JsonDataRepository factory");
            var result = new JsonDataRepositoryFactory();
            Logger.Debug("Done building session factory");
            return result;
        }
    }
}

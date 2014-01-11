using Orchard.Environment;
using Orchard.Environment.Configuration;
using Orchard.Environment.ShellBuilders.Models;
using Orchard.Localization;
using Orchard.Logging;
using System;
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
        private readonly ShellSettings _shellSettings;
        private readonly ShellBlueprint _shellBlueprint;
        private readonly IHostEnvironment _hostEnvironment;

        public JsonDataRepositoryFactoryHolder(
            ShellSettings shellSettings,
            ShellBlueprint shellBlueprint,
            IHostEnvironment hostEnvironment)
        {
            _shellSettings = shellSettings;
            _shellBlueprint = shellBlueprint;
            _hostEnvironment = hostEnvironment;

            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        public Localizer T { get; set; }
        public ILogger Logger { get; set; }


        private IJsonDataRepositoryFactory _jsonDataRepositoryFactory;
        public IJsonDataRepositoryFactory GetRepositoryFactory()
        {
            lock (this)
            {
                if (_jsonDataRepositoryFactory == null)
                {
                    _jsonDataRepositoryFactory = BuildRepositoryFactory();
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

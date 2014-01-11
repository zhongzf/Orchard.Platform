using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Linq;
using Orchard.Logging;
using Orchard.Utility.Extensions;
using Orchard.Environment.Configuration;
using Orchard.Environment.ShellBuilders.Models;
using Orchard.FileSystems.AppData;
using System.IO;

namespace Orchard.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ISessionLocator _sessionLocator;
        private readonly ShellSettings _shellSettings;
        private readonly ShellBlueprint _shellBlueprint;
        private readonly IAppDataFolder _appDataFolder;
        private readonly IJsonDataRepositoryFactoryHolder _jsonDataRepositoryFactoryHolder;

        public Repository(ISessionLocator sessionLocator,
            ShellSettings shellSettings,
            ShellBlueprint shellBlueprint,
            IAppDataFolder appDataFolder,
            IJsonDataRepositoryFactoryHolder jsonDataRepositoryFactoryHolder)
        {
            _sessionLocator = sessionLocator;
            _shellSettings = shellSettings;
            _shellBlueprint = shellBlueprint;
            _appDataFolder = appDataFolder;
            _jsonDataRepositoryFactoryHolder = jsonDataRepositoryFactoryHolder;
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }


        private bool IsDefinedInDataFile()
        {
            return _shellSettings.DataFiles.Contains(_shellBlueprint.Records.SingleOrDefault(r => r.Type == typeof(T)).TableName);
        }

        private string GetJsonDataFileName()
        {
            string tableName = _shellBlueprint.Records.SingleOrDefault(r => r.Type == typeof(T)).TableName;
            var jsonDataPath = _appDataFolder.Combine("Sites", _shellSettings.Name, "Data");
            _appDataFolder.CreateDirectory(jsonDataPath);
            var jsonDataFolder = _appDataFolder.MapPath(jsonDataPath);
            return Path.Combine(jsonDataFolder, tableName);
        }

        private IJsonDataRepositoryFactory jsonDataRepositoryFactory;
        public IRepository<T> JsonDataRepository
        {
            get
            {
                if (jsonDataRepositoryFactory == null)
                {
                    jsonDataRepositoryFactory = _jsonDataRepositoryFactoryHolder.GetRepositoryFactory();
                    jsonDataRepositoryFactory.BindFile(typeof(T), GetJsonDataFileName());
                }
                return jsonDataRepositoryFactory.GetRepository<T>();
            }
        }

        protected virtual ISessionLocator SessionLocator
        {
            get { return _sessionLocator; }
        }

        protected virtual ISession Session
        {
            get { return SessionLocator.For(typeof(T)); }
        }

        public virtual IQueryable<T> Table
        {
            get
            {
                if (IsDefinedInDataFile())
                {
                    return JsonDataRepository.Table;
                }
                return Session.Query<T>().Cacheable();
            }
        }


        #region IRepository<T> Members

        void IRepository<T>.Create(T entity)
        {
            Create(entity);
        }

        void IRepository<T>.Update(T entity)
        {
            Update(entity);
        }

        void IRepository<T>.Delete(T entity)
        {
            Delete(entity);
        }

        void IRepository<T>.Copy(T source, T target)
        {
            Copy(source, target);
        }

        void IRepository<T>.Flush()
        {
            Flush();
        }

        T IRepository<T>.Get(int id)
        {
            return Get(id);
        }

        T IRepository<T>.Get(Expression<Func<T, bool>> predicate)
        {
            return Get(predicate);
        }

        IQueryable<T> IRepository<T>.Table
        {
            get { return Table; }
        }

        int IRepository<T>.Count(Expression<Func<T, bool>> predicate)
        {
            return Count(predicate);
        }

        IEnumerable<T> IRepository<T>.Fetch(Expression<Func<T, bool>> predicate)
        {
            return Fetch(predicate).ToReadOnlyCollection();
        }

        IEnumerable<T> IRepository<T>.Fetch(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order)
        {
            return Fetch(predicate, order).ToReadOnlyCollection();
        }

        IEnumerable<T> IRepository<T>.Fetch(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order, int skip,
                                            int count)
        {
            return Fetch(predicate, order, skip, count).ToReadOnlyCollection();
        }

        #endregion

        public virtual T Get(int id)
        {
            if (IsDefinedInDataFile())
            {
                return JsonDataRepository.Get(id);
            }
            return Session.Get<T>(id);
        }

        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            return Fetch(predicate).SingleOrDefault();
        }

        public virtual void Create(T entity)
        {
            Logger.Debug("Create {0}", entity);
            if (IsDefinedInDataFile())
            {
                JsonDataRepository.Create(entity);
            }
            else
            {
                Session.Save(entity);
            }
        }

        public virtual void Update(T entity)
        {
            Logger.Debug("Update {0}", entity);
            if (IsDefinedInDataFile())
            {
                JsonDataRepository.Update(entity);
            }
            else
            {
                Session.Evict(entity);
                Session.Merge(entity);
            }
        }

        public virtual void Delete(T entity)
        {
            Logger.Debug("Delete {0}", entity);
            if (IsDefinedInDataFile())
            {
                JsonDataRepository.Delete(entity);
            }
            else
            {
                Session.Delete(entity);
            }
        }

        public virtual void Copy(T source, T target)
        {
            Logger.Debug("Copy {0} {1}", source, target);
            var metadata = Session.SessionFactory.GetClassMetadata(typeof(T));
            var values = metadata.GetPropertyValues(source, EntityMode.Poco);
            metadata.SetPropertyValues(target, values, EntityMode.Poco);
        }

        public virtual void Flush()
        {
            if (IsDefinedInDataFile())
            {
                JsonDataRepository.Flush();
            }
            else
            {
                Session.Flush();
            }
        }

        public virtual int Count(Expression<Func<T, bool>> predicate)
        {
            return Fetch(predicate).Count();
        }

        public virtual IQueryable<T> Fetch(Expression<Func<T, bool>> predicate)
        {
            return Table.Where(predicate);
        }

        public virtual IQueryable<T> Fetch(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order)
        {
            var orderable = new Orderable<T>(Fetch(predicate));
            order(orderable);
            return orderable.Queryable;
        }

        public virtual IQueryable<T> Fetch(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order, int skip,
                                           int count)
        {
            return Fetch(predicate, order).Skip(skip).Take(count);
        }
    }
}
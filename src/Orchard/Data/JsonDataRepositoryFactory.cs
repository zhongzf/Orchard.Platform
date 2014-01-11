using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orchard.Data
{
    public interface IJsonDataRepositoryFactory
    {
        IRepository<T> GetRepository<T>() where T : class;
        void BindFile(Type type, string fileName);
    }

    public class JsonDataRepositoryFactory : IJsonDataRepositoryFactory
    {
        private ConcurrentDictionary<Type, object> repositories = new ConcurrentDictionary<Type, object>();
        private ConcurrentDictionary<Type, string> fileNames = new ConcurrentDictionary<Type, string>();
        public ConcurrentDictionary<Type, string> FileNames
        {
            get
            {
                return fileNames;
            }
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            return repositories.GetOrAdd(typeof(T), _ => { return new JsonDataRepository<T>(fileNames.GetOrAdd(typeof(T), string.Empty)); }) as IRepository<T>;
        }

        public void BindFile(Type type, string fileName)
        {
            FileNames[type] = fileName;
        }
    }
}

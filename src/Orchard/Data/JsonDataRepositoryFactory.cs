using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Orchard.Data
{
    public interface IJsonDataRepositoryFactory
    {
        IRepository<T> GetRepository<T>() where T : class;
        void BindFile(Type type, string fileName);
        IEnumerable<Action<T, T>> GetPropertyTransfers<T>();
    }

    public class JsonDataRepositoryFactory : IJsonDataRepositoryFactory
    {
        private ConcurrentDictionary<Type, object> repositories = new ConcurrentDictionary<Type, object>();
        private ConcurrentDictionary<Type, object> propertyTransfers = new ConcurrentDictionary<Type, object>();
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
            return repositories.GetOrAdd(typeof(T), _ => { return new JsonDataRepository<T>(fileNames.GetOrAdd(typeof(T), string.Empty), this); }) as IRepository<T>;
        }

        public void BindFile(Type type, string fileName)
        {
            if (!FileNames.ContainsKey(type))
            {
                FileNames[type] = fileName;
            }
        }

        public static IEnumerable<Action<S, T>> BuildPropertyTransfers<S, T>()
        {
            List<Action<S, T>> propertyTransferList = new List<Action<S, T>>();
            Type sourceType = typeof(S);
            Type targetType = typeof(T);
            var properties = sourceType.GetProperties();
            foreach (var propertyInfo in properties)
            {
                string propertyName = propertyInfo.Name;
                var targetPropertyInfo = targetType.GetProperty(propertyName);
                if (targetPropertyInfo != null)
                {
                    var source = Expression.Parameter(sourceType, "source");
                    var target = Expression.Parameter(targetType, "target");
                    var source_get_call = Expression.Call(source, propertyInfo.GetGetMethod());
                    var target_set_call = Expression.Call(target, targetPropertyInfo.GetSetMethod(), source_get_call);
                    var expression = Expression.Lambda<Action<S, T>>(target_set_call, source, target);
                    var action = expression.Compile();
                    propertyTransferList.Add(action);
                }
            }
            return propertyTransferList;
        }

        public IEnumerable<Action<T, T>> GetPropertyTransfers<T>()
        {
            return propertyTransfers.GetOrAdd(typeof(T), _ => { return BuildPropertyTransfers<T, T>(); }) as IEnumerable<Action<T, T>>;
        }
    }
}

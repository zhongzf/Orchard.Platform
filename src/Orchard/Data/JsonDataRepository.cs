using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.Logging;
using System.Linq.Expressions;
using Newtonsoft.Json;
using System.IO;
using Orchard.Utility.Extensions;
using System.Collections.Concurrent;

namespace Orchard.Data
{
    public class JsonDataRepository<T> : IRepository<T> where T : class
    {
        public ILogger Logger { get; set; }
        private readonly IJsonDataRepositoryFactory _factory;

        private string fileName;
        public string FileName
        {
            get
            {
                return this.fileName;
            }
            set
            {
                this.fileName = value;
            }
        }

        public JsonDataRepository(string fileName, IJsonDataRepositoryFactory factory)
            : this()
        {
            this.fileName = fileName;
            this._factory = factory;
        }

        public JsonDataRepository()
        {
            Logger = NullLogger.Instance;
        }

        private bool loaded = false;
        private ConcurrentDictionary<int, T> data;

        public virtual IQueryable<T> Table
        {
            get
            {
                if (!this.loaded)
                {
                    lock (this)
                    {
                        if (!this.loaded)
                        {
                            if (File.Exists(fileName))
                            {
                                string json = File.ReadAllText(fileName);
                                var list = JsonConvert.DeserializeObject<List<T>>(json);
                                data = new ConcurrentDictionary<int, T>();
                                foreach (var value in list)
                                {
                                    data.TryAdd(GetId(value), value);
                                }
                            }
                            else
                            {
                                data = new ConcurrentDictionary<int, T>();
                            }
                            this.loaded = true;
                        }
                    }
                }
                return data.Values.AsQueryable();
            }
        }

        public virtual int GetId(T entity)
        {
            dynamic t = entity;
            return (int)t.Id;
        }

        public virtual T SetId(T entity, int id)
        {
            dynamic t = entity;
            t.Id = id;
            return (T)t;
        }

        public virtual int GetUniqueId()
        {
            if (data.Keys.Count > 0)
            {
                return data.Keys.Max() + 1;
            }
            else
            {
                return 1;
            }
        }

        public virtual void Create(T entity)
        {
            Logger.Debug("Create {0}", entity);
            int id = GetUniqueId();
            data.TryAdd(id, SetId(entity, id));
            try
            {
                Flush();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "JsonDataRepository Flush failed.");
            }
        }

        public virtual void Update(T entity)
        {
            Logger.Debug("Update {0}", entity);
            if (!data.Values.Contains(entity))
            {
                Copy(entity, Get(GetId(entity)));
            }
            try
            {
                Flush();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "JsonDataRepository Flush failed.");
            }
        }

        public virtual void Delete(T entity)
        {
            Logger.Debug("Delete {0}", entity);
            T t;
            data.TryRemove(GetId(entity), out t);
            try
            {
                Flush();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "JsonDataRepository Flush failed.");
            }
        }

        public virtual void Flush()
        {
            lock (this)
            {
                string json = JsonConvert.SerializeObject(data.Values);
                File.WriteAllText(fileName, json);
            }
        }


        public void Copy(T source, T target)
        {
            Logger.Debug("Copy {0} {1}", source, target);
            IEnumerable<Action<T, T>> propertyTransferList = _factory.GetPropertyTransfers<T>();
            foreach (var propertyTransfer in propertyTransferList)
            {
                propertyTransfer(source, target);
            }
        }

        public T Get(int id)
        {
            var param = Expression.Parameter(typeof(T), "t");
            var body = Expression.Equal
                (
                    Expression.PropertyOrField(param, "Id"),
                    Expression.Constant(id)
                );
            var predicate = Expression.Lambda<Func<T, bool>>(body, param);
            return Get(predicate);
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return Fetch(predicate).SingleOrDefault();
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

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.Logging;
using System.Linq.Expressions;
using Newtonsoft.Json;
using System.IO;
using Orchard.Utility.Extensions;

namespace Orchard.Data
{
    public class JsonDataRepository<T> : IRepository<T> where T : class
    {
        public ILogger Logger { get; set; }

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

        public JsonDataRepository(string fileName) : this()
        {
            this.fileName = fileName;
        }

        public JsonDataRepository()
        {
            Logger = NullLogger.Instance;
        }

        private bool loaded = false;
        private List<T> data;

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
                                data = JsonConvert.DeserializeObject<List<T>>(json);
                            }
                            else
                            {
                                data = new List<T>();
                            }
                            this.loaded = true;
                        }
                    }
                }
                return data.AsQueryable();
            }
        }

        public virtual void Create(T entity)
        {
            Logger.Debug("Create {0}", entity);
            data.Add(entity);
        }

        public virtual void Update(T entity)
        {
            Logger.Debug("Update {0}", entity);
            // TODO:
        }

        public virtual void Delete(T entity)
        {
            Logger.Debug("Delete {0}", entity);
            data.Remove(entity);
        }

        public virtual void Flush()
        {
            string json = JsonConvert.SerializeObject(data);
            File.WriteAllText(fileName, json);
        }
        

        public void Copy(T source, T target)
        {
            Logger.Debug("Copy {0} {1}", source, target);
            // TODO:
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

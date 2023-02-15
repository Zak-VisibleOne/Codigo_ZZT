using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Text.RegularExpressions;

namespace POSData
{
    public class BaseData<T> : IBaseData<T> where T : class
    {
        #region Fields

        private DbContext context;

        private IDbSet<T> entities;

        private readonly Dictionary<ObjectContext, DbContext> contexts = new Dictionary<ObjectContext, DbContext>();

        #endregion         

        #region Constructors and Destructors
        public BaseData(bool proxy_enable = true)
        {
            this.context = new POSSystemEntities();
            this.context.Configuration.ProxyCreationEnabled = proxy_enable;
        }
        #endregion

        #region Public Properties

        // public bool AutoCommitEnabled { get; set; }

        public DbContext Context
        {
            get
            {
                return this.context;
            }
        }

        public virtual IQueryable<T> Table
        {
            get
            {
                return this.Entities.AsNoTracking();
            }
        }


        #endregion

        #region Properties

        private DbSet<T> Entities
        {
            get
            {
                if (this.entities == null)
                {
                    this.entities = this.context.Set<T>();
                }

                return this.entities as DbSet<T>;
            }
        }

        #endregion

        #region Public Methods and Operators



        public T Create()
        {
            return this.Entities.Create();
        }


        public string GetTableName()
        {
            string table = context.GetTableName<T>();
            return table;
        }


        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            if (this.Context.Entry(entity).State == EntityState.Detached)
            {
                this.Entities.Attach(entity);
            }

            this.Entities.Remove(entity);

            //if (this.AutoCommitEnabled)
            //{
            this.context.SaveChanges();
            //}
        }

        public List<T> GetDataByQuery(string qry, object[] par)
        {
            IEnumerable<T> rows = context.Database.SqlQuery<T>(qry, par);
            return rows.ToList();
        }

        public T GetById(object id)
        {
            return this.Entities.Find(id);
        }

        public void Insert(T entity)
        {

            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            this.Entities.Add(entity);
            this.context.SaveChanges();
        }

        public void InsertRange(List<T> list, int batchSize = 100)
        {
            try
            {
                if (entities == null)
                {
                    throw new ArgumentNullException("list");
                }

                if (entities.Any())
                {
                    if (batchSize <= 0)
                    {
                        // insert all in one step
                        list.ForEach(x => this.Entities.Add(x));
                        //if (this.AutoCommitEnabled)
                        // {
                        this.context.SaveChanges();
                        //}
                    }
                    else
                    {
                        int i = 1;
                        bool saved = false;
                        foreach (var entity in entities)
                        {
                            this.Entities.Add(entity);
                            saved = false;
                            if (i % batchSize == 0)
                            {
                                // if (this.AutoCommitEnabled)
                                //  {
                                this.context.SaveChanges();
                                // }

                                i = 0;
                                saved = true;
                            }

                            i++;
                        }

                        if (!saved)
                        {
                            //if (this.AutoCommitEnabled)
                            // {
                            this.context.SaveChanges();
                            // }
                        }
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                throw ex;
            }
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            this.Entities.Attach(entity);
            this.Context.Entry(entity).State = EntityState.Modified;
            this.context.SaveChanges();
        }

        public void UpdateRange(List<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            foreach (var entity in list)
            {
                this.Entities.Attach(entity);
                this.Context.Entry(entity).State = EntityState.Modified;
            }

            // if (this.AutoCommitEnabled)
            // {
            this.context.SaveChanges();
            // }
        }

        #endregion

    }

    public static class ContextExtensions
    {
        public static string GetTableName<T>(this DbContext context) where T : class
        {
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;

            return objectContext.GetTableName<T>();
        }

        public static string GetTableName<T>(this ObjectContext context) where T : class
        {
            string sql = context.CreateObjectSet<T>().ToTraceString();
            Regex regex = new Regex("FROM (?<table>.*) AS");
            Match match = regex.Match(sql);

            string table = match.Groups["table"].Value;
            return table;
        }
    }
}

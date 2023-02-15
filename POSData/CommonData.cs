using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace POSData
{
    public class CommonData<T> where T : class
    {
        bool ProxyEnable = true;
        public CommonData(bool proxy = true)
        {
            ProxyEnable = proxy;
        }

        public List<T> GetDataByQuery(string qry, object[] par)
        {
            return new BaseData<T>(ProxyEnable).GetDataByQuery(qry, par);
        }

        public T GetRecord(Expression<Func<T, bool>> filter = null)
        {
            if (filter == null)
            {
                return new BaseData<T>(ProxyEnable).Table.FirstOrDefault();
            }
            return new BaseData<T>(ProxyEnable).Table.FirstOrDefault(filter);
        }

        public List<T> GetRecords(Expression<Func<T, bool>> filter = null)
        {
            if (filter == null)
            {
                return new BaseData<T>(ProxyEnable).Table.ToList();
            }
            return new BaseData<T>(ProxyEnable).Table.Where(filter).ToList();
        }

        public T GetNext<TOrderBy>(Expression<Func<T, bool>> filter, Expression<Func<T, TOrderBy>> orderBy)
        {
            if (filter == null) return GetFirst();
            //T current = new BaseData<T>().Table.Where(filter).FirstOrDefault();
            List<T> var = new BaseData<T>(ProxyEnable).Table.OrderBy(orderBy).ToList();
            T current = var.AsQueryable().SingleOrDefault(filter);
            var next = GetNext<T>(var, current);
            return next;
        }

        public T GetPrevious<TOrderBy>(Expression<Func<T, bool>> filter, Expression<Func<T, TOrderBy>> orderBy)
        {
            List<T> var = new BaseData<T>(ProxyEnable).Table.OrderBy(orderBy).ToList();
            T current = var.AsQueryable().SingleOrDefault(filter);
            var prev = GetPrevious<T>(var, current);
            return prev;
        }

        public T GetFirst()
        {
            T current = new BaseData<T>(ProxyEnable).Table.FirstOrDefault();
            return current;
        }

        public static T GetNext<T>(List<T> list, T current)
        {
            try
            {
                return list.SkipWhile(x => !x.Equals(current)).Skip(1).First();
            }
            catch
            {
                return default(T);
            }
        }

        public static T GetPrevious<T>(List<T> list, T current)
        {
            try
            {
                return list.TakeWhile(x => !x.Equals(current)).Last();
            }
            catch
            {
                return default(T);
            }
        }

        public void InsertEntity<T>(T entity) where T : class
        {
            new BaseData<T>(ProxyEnable).Insert(entity);
        }

        public void UpdateEntity(T entity)
        {
            new BaseData<T>(ProxyEnable).Update(entity);
        }

        public void DeleteEntity(T entity)
        {
            new BaseData<T>(ProxyEnable).Delete(entity);
        }
    }
}

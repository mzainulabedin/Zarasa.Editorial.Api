using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Zarasa.Editorial.Api.Common.EventArgs;
using Zarasa.Editorial.Api.Data;
using Zarasa.Editorial.Api.Models;

namespace Zarasa.Editorial.Api.Repository
{
    public delegate void SearchEventHandler<T>(SearchEventArgs<T> e);

    public abstract class EntityRepository<T> where T : Entity
    {

        private readonly ApplicationDbContext _context;

        public EntityRepository(ApplicationDbContext context) => _context = context;

        protected ApplicationDbContext getContext() {
            return _context;
        }

        protected abstract DbSet<T> GetDbSet();


        public IEnumerable<T> Get(string searchString, string orderBy, string orderByDirection, int? page, ref int? size, out long count)
        {
            var query = GetQuery(searchString, orderBy, orderByDirection);
            
            query = paging(query, page, ref size, out count);
            
            return query.ToList();
        }

        protected IQueryable<T> GetQuery(string searchString, string orderBy, string orderByDirection)
        {
            var query = GetDbSet().Where(x => !x.is_deleted).AsQueryable();
            
            var searchEventArgs = new SearchEventArgs<T>(query, searchString, orderBy, orderByDirection);
            OnSearch(searchEventArgs);
            query = searchEventArgs.Query;
            
            return query;
        }

        protected IQueryable<T> paging(IQueryable<T> queryable, int? page, ref int? size, out long count){
            if(page != null && page != 0)
            {
                if(size == null || size == 0)
                {
                    size = 20;
                }
                count = queryable.Count();
                queryable = queryable.Skip((page.Value - 1) * size.Value).Take(size.Value).AsQueryable();
            } else {
                count = 0;
            }
            return queryable;
        }

        public event SearchEventHandler<T> searchEventHandler;
        protected virtual void OnSearch(SearchEventArgs<T> e)
        {
            SearchEventHandler<T> handler = searchEventHandler;
            if (handler != null)
            {
                handler(e);
            }
        }

        public T Get(long id) => GetDbSet().Where(x => x.id == id).FirstOrDefault();

        public T Create(T entity, long? currentUserId = null) {
            if(currentUserId.HasValue){
                entity.created_by = currentUserId;
            }
            GetDbSet().Add(entity);
            getContext().SaveChanges();
            return entity;
        }

        public T Update(long id, T updatedEntity, long? currentUserId = null) {
            var entity = Get(id);
            if (entity == null)
            {
                throw new Exception("Record not Exists");
            }
            entity.copy(updatedEntity);
            if(currentUserId.HasValue){    
                entity.updated_at = DateTime.UtcNow;
            }
            entity.updated_by = currentUserId;

            GetDbSet().Update(entity);
            getContext().SaveChanges();

            return entity;
        }

        public T Delete(long id) {
            var entity = Get(id);
            if (entity == null)
            {
                throw new Exception("Record not Exists");
            }
            entity.is_deleted = true;
            entity.updated_at = DateTime.UtcNow;
            GetDbSet().Update(entity);
            getContext().SaveChanges();
            return entity;
        }
        
    }
}
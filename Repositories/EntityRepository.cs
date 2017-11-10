using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Zarasa.Editorial.Api.Data;
using Zarasa.Editorial.Api.Models;

namespace Zarasa.Editorial.Api.Repository
{
    public abstract class EntityRepository<T> where T : Entity
    {

        private readonly ApplicationDbContext _context;

        public EntityRepository(ApplicationDbContext context) => _context = context;

        protected ApplicationDbContext getContext() {
            return _context;
        }

        protected abstract DbSet<T> GetDbSet();


        public IEnumerable<T> Get(int? page, ref int? size, out long count)
        {
            var query = GetDbSet().Where(x => !x.is_deleted).AsQueryable();
            if(page != null && page != 0)
            {
                if(size == null || size == 0)
                {
                    size = 20;
                }
                count = query.Count();
                query = query.Skip((page.Value - 1) * size.Value).Take(size.Value).AsQueryable();
            } else {
                count = 0;
            }
            return query.ToList();
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
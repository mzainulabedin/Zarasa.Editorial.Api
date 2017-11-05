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


        public IEnumerable<T> Get() => GetDbSet().Where(x => !x.is_deleted).ToList();

        public T Get(long id) => GetDbSet().Where(x => x.id == id).FirstOrDefault();

        public T Create(T entity, long currentUserId) {
            entity.created_by = currentUserId;
            GetDbSet().Add(entity);
            getContext().SaveChanges();
            return entity;
        }

        public T Update(long id, T updatedEntity, long currentUserId) {
            var entity = Get(id);
            if (entity == null)
            {
                throw new Exception("Record not Exists");
            }
            entity.copy(updatedEntity);
                
            entity.updated_at = DateTime.UtcNow;
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
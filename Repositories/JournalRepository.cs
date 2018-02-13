using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zarasa.Editorial.Api.Data;
using Zarasa.Editorial.Api.Models;
using Zarasa.Editorial.Api.Common.Extensions;
using Zarasa.Editorial.Api.Common.EventArgs;

namespace Zarasa.Editorial.Api.Repository
{
    public class JournalRepository : EntityRepository<Journal>
    {
        public JournalRepository(ApplicationDbContext context) : base(context)
        {
        }

        protected override DbSet<Journal> GetDbSet() => getContext().Journals;

        
        public IEnumerable<Journal> GetByName(string searchString, string orderBy, string orderByDirection, Journal.JournalStatus? status, int? page, ref int? size, out long count)
        {
            var query = GetQuery(searchString, orderBy, orderByDirection);

            if(status.HasValue && status.Value == Journal.JournalStatus.Panding){
                query = query.Where(x => x.status == Journal.JournalStatus.Panding).AsQueryable();
            } else {
                query = query.Where(x => x.status == Journal.JournalStatus.Active).AsQueryable();
            }
            
            query = paging(query, page, ref size, out count);

            return query.ToList();
        }

        public Journal Activate(long id, long currentUserId) {
            var entity = Get(id);
            if (entity == null)
            {
                throw new Exception("Record not Exists");
            }
            entity.status = Journal.JournalStatus.Active;
            entity.updated_at = DateTime.UtcNow;
            entity.updated_by = currentUserId;

            GetDbSet().Update(entity);
            getContext().SaveChanges();

            return entity;
        }

        protected override void OnSearch(Common.EventArgs.SearchEventArgs<Journal> e) 
        {
            if(! string.IsNullOrWhiteSpace(e.SearchString)) 
            {
                e.Query = e.Query.Where(x => x.name.Contains(e.SearchString) || x.organization_name.Contains(e.SearchString) || x.detail.Contains(e.SearchString));
            }
            if(!string.IsNullOrEmpty(e.OrderBy))
            {
                if((new string[]{"name", "organization_name", "detail"}).Contains(e.OrderBy.ToLower()))
                {
                    e.Query = e.Query.OrderBy(e.OrderBy.ToLower(), e.OrderByDirection.ToLower());
                }
            }
        }
    }
}
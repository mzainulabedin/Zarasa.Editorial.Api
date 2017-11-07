using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zarasa.Editorial.Api.Data;
using Zarasa.Editorial.Api.Models;

namespace Zarasa.Editorial.Api.Repository
{
    public class JournalRepository : EntityRepository<Journal>
    {
        public JournalRepository(ApplicationDbContext context) : base(context)
        {
        }

        protected override DbSet<Journal> GetDbSet() => getContext().Journals;

        
        public IEnumerable<Journal> GetByName(string name, int? page, ref int? size, out long count)
        {
            var query = GetDbSet().Where(x => !x.is_deleted).AsQueryable();
            if (name != null)
            {
                query = query.Where(x => x.name.Contains(name)).AsQueryable();
            }
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
    }
}
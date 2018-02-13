using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Zarasa.Editorial.Api.Data;
using Zarasa.Editorial.Api.Models;
using Zarasa.Editorial.Api.Common.Extensions;

namespace Zarasa.Editorial.Api.Repository
{
    public class UserRepository : EntityRepository<User>
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        protected override DbSet<User> GetDbSet() => getContext().Users;

        public bool CredentialExists(string username, string password){
            return GetDbSet().Any(x => !x.is_deleted && x.email == username && x.password == password);
        }

        public bool EmailExists(string email, long? id = null){
            if(id == null){
                return GetDbSet().Any(x => !x.is_deleted && x.email == email);
            } else {
                return GetDbSet().Any(x => !x.is_deleted && x.email == email && x.id != id);
            }
        }

        public User GetByEmail(string username){
            return GetDbSet().Where(x => !x.is_deleted && x.email == username).FirstOrDefault();
        }

        protected override void OnSearch(Common.EventArgs.SearchEventArgs<User> e) 
        {
            if(! string.IsNullOrWhiteSpace(e.SearchString)) 
            {
                e.Query = e.Query.Where(x => x.first_name.Contains(e.SearchString) || x.last_name.Contains(e.SearchString) || x.email.Contains(e.SearchString));
            }
            if(!string.IsNullOrEmpty(e.OrderBy))
            {
                if((new string[]{"first_name", "last_name", "email"}).Contains(e.OrderBy.ToLower()))
                {
                    e.Query = e.Query.OrderBy(e.OrderBy.ToLower(), e.OrderByDirection.ToLower());
                }
            }
        }
    }
}
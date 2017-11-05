using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zarasa.Editorial.Api.Data;
using Zarasa.Editorial.Api.Models;

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
    }
}
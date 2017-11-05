
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zarasa.Editorial.Api.Models
{
    public class Entity
    {
        [Column("id")]public long id { get; set; }

        private DateTime _created_at = DateTime.UtcNow;
        public DateTime created_at
        {
            get { return _created_at;}
            set { _created_at = value;}
        }
        
        
        public long? created_by { get; set; }
        
        private DateTime _updated_at = DateTime.UtcNow;
        public DateTime updated_at
        {
            get { return _updated_at;}
            set { _updated_at = value;}
        }
        

        public long? updated_by { get; set; }
        
        public bool is_deleted { get; set; }
        
        
        public virtual void copy(Entity entity){

        }
    }
}
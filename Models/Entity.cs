
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zarasa.Editorial.Api.Models
{
    public class Entity
    {
        [Key]
        [Column("id", Order = 1)]
        public long id { get; set; }

        [Column(Order = 2)]
        private DateTime _created_at = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        [Column(Order = 3)]
        public DateTime created_at
        {
            get { return _created_at;}
            set { _created_at = value;}
        }
        
        [Column(Order = 3)]
        public long? created_by { get; set; }
        
        private DateTime _updated_at = DateTime.UtcNow;
        [DataType(DataType.DateTime)]
        [Display(Name = "Update At")]
        [Column(Order = 4)]
        public DateTime updated_at
        {
            get { return _updated_at;}
            set { _updated_at = value;}
        }
        
        [Column(Order = 5)]
        public long? updated_by { get; set; }
        
        [Column(Order = 6)]
        public bool is_deleted { get; set; }
        
        
        public virtual void copy(Entity entity){

        }
    }
}
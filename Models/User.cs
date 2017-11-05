using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Zarasa.Editorial.Api.Models
{
    [Table("users")]
    public class User : Entity
    {
        [Required]
        [StringLength(30,  ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        public string first_name { get; set; }
        
        [Required]
        [StringLength(30,  ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string last_name { get; set; }
        
        private bool _is_active = true;
        public bool is_active
        {
            get { return _is_active;}
            set { _is_active = value;}
        }
        
        [Required]
        [EmailAddress]
        [StringLength(100,  ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Email")]
        public string email { get; set; }   
        

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }

        public override void copy(Entity entity){
            var user = (User)entity;
            
            this.first_name = user.first_name;
            this.last_name = user.last_name;
            this.email = user.email;
            this.is_active = user.is_active;
        }
    }

    
}
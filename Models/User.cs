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
        [Column(Order = 7)]
        public string first_name { get; set; }
        
        [Required]
        [StringLength(30,  ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        [Column(Order = 8)]
        public string last_name { get; set; }
        
        private bool _is_active = true;
        [Required]
        [Display(Name = "Is Active")]
        [Column(Order = 9)]
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
        [Column(Order = 10)]
        public string email { get; set; }   
        

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Column(Order = 11)]
        public string password { get; set; }

        [Column(Order = 12)]
        public string salt { get; set; }

        
        [Column(Order = 13)]
        [ForeignKey("journal_id")]
        public virtual Journal journal { get; set; }

        [Required]
        [Display(Name = "User Type")]
        [Column(Order = 14)]
        public UserTypeEnum user_type { get; set; }


        public override void copy(Entity entity){
            var user = (User)entity;
            
            this.first_name = user.first_name;
            this.last_name = user.last_name;
            this.email = user.email;
            this.is_active = user.is_active;

            
        }

        public enum UserTypeEnum{
            Admin = 0,
            Editor = 1,
            AssistantEditor = 2,
            Author = 3,
            Reviewer = 4
        }
    }

    
}
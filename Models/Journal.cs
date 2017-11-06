using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Zarasa.Editorial.Api.Models
{
    [Table("journals")]
    public class Journal : Entity
    {
        [Required]
        [StringLength(100,  ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        [Column(Order = 7)]
        public string name { get; set; }

        [Required]
        [StringLength(6,  ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Code")]
        [Column(Order = 8)]
        public string code { get; set; }

        [StringLength(100,  ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 0)]
        [DataType(DataType.Text)]
        [Display(Name = "Organization Name")]
        [Column(Order = 9)]
        public string organization_name { get; set; }

        [StringLength(2000,  ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 0)]
        [DataType(DataType.Text)]
        [Display(Name = "Detail")]
        [Column(Order = 10)]
        public string detail { get; set; }

        [Required]
        [Display(Name = "Status")]
        [Column(Order = 11)]
        public int status { get; set; }


        [EmailAddress]
        [StringLength(100,  ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Email")]
        [Column(Order = 10)]
        public string admin_email { get; set; }


        public virtual ICollection<User> users { get; set; }
    }
}
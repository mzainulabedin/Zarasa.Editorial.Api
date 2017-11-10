using System.ComponentModel.DataAnnotations;
using Zarasa.Editorial.Api.Models;

namespace Zarasa.Editorial.Api.Request
{
    public class JournalRequest
    {
        [Required]
        [StringLength(100,  ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Name")]
        public string name { get; set; }

        [Required]
        [StringLength(6,  ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Code")]
        public string code { get; set; }

        [StringLength(100,  ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 0)]
        [Display(Name = "Organization Name")]
        public string organization_name { get; set; }

        [StringLength(2000,  ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 0)]
        [Display(Name = "Detail")]
        public string detail { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100,  ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [Display(Name = "Email")]
        public virtual string admin_email { get; set; }

        public Journal toJournal(){
            var journal = new Journal();

            journal.name = this.name;
            journal.code = this.code;
            journal.organization_name = this.organization_name;
            journal.detail = this.detail;
            journal.admin_email = this.admin_email;
            journal.status = Journal.JournalStatus.Panding;
            
            return journal;
        }
    }
}
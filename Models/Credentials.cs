using System.ComponentModel.DataAnnotations;

namespace Zarasa.Editorial.Api.Models
{
  public class Credentials {
    [Required]
    [EmailAddress]
    [Display(Name = "Username")]
    public string username { get; set; }

    [Required]
    [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string password { get; set; }
  }
}
using System.ComponentModel.DataAnnotations;

namespace Pierres.ViewModels
{
  public class RegisterViewModel
  {
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required]
    [Display(Name = "UserName")]
    public string UserName { get; set; }

    [Required]
    [Display(Name = "FirstName")]
    public string FirstName { get; set; }

    [Required]
    [Display(Name = "LastName")]
    public string LastName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
  }
}
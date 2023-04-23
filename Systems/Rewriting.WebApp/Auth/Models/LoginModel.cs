using System.ComponentModel.DataAnnotations;

namespace Rewriting.WebApp;

public class LoginModel
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email")]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}

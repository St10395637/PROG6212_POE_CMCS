using System.ComponentModel.DataAnnotations;

namespace Claiming_System.Models
{
    // ViewModel used for admin registration
    public class AdminRegisterViewModel
    {
        // Name of the admin. This field is required and has a maximum length of 100 characters.
        [Required] // This attribute makes the field mandatory.
        [StringLength(100)] // Ensures that the name is no longer than 100 characters.
        public string Name { get; set; }

        // Email address of the admin. This field is required and must be a valid email format.
        [Required] // This attribute makes the field mandatory.
        [EmailAddress] // Ensures the input is a valid email address format.
        public string Email { get; set; }

        // Password for the admin account. This field is required, and the password must be between 6 and 100 characters long.
        [Required] // This attribute makes the field mandatory.
        [StringLength(100, MinimumLength = 6)] // Ensures the password is at least 6 characters long but no longer than 100 characters.
        public string Password { get; set; }

        // Role of the admin (e.g., Admin, SuperAdmin). This field is required.
        [Required] // This attribute makes the field mandatory.
        public string Role { get; set; }

        // Additional fields can be added as necessary, depending on further requirements.
    }
}

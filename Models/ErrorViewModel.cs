namespace Claiming_System.Models
{
    // The Admin class represents an administrator user in the claiming system.
    public class Admin
    {
        // The Name property stores the full name of the admin.
        public string Name { get; set; }

        // The Email property stores the email address of the admin.
        // This email can be used for login and communication purposes.
        public string Email { get; set; }

        // The Password property stores the password of the admin.
        // In a real-world scenario, this should never be stored as plain text.
        // It should be hashed and salted for security.
        public string Password { get; set; }
    }
}

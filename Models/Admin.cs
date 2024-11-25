namespace Claiming_System.Models
{
    // The Admin class represents an administrator in the claiming system
    public class Admin
    {
        // Property to store the name of the admin
        public string Name { get; set; }

        // Property to store the email of the admin
        public string Email { get; set; }

        // Property to store the password of the admin
        // In production, passwords should be hashed and not stored as plain text
        public string Password { get; set; }
    }
}

// Using statements for necessary namespaces
using Claiming_System.Models;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

public class ApplicationDbContext : DbContext
{
    // Constructor that passes the DbContextOptions to the base class constructor
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets represent collections of entities (tables) in the database
    
    // Represents the 'Users' table in the database
    public DbSet<User> Users { get; set; }
    
    // Represents the 'Lecturers' table in the database
    public DbSet<Lecturer> Lecturers { get; set; }
    
    // Represents the 'Claims' table in the database
    public DbSet<Claim> Claims { get; set; }
    
    // Represents the 'SupportingDocuments' table in the database
    public DbSet<SupportingDocuments> SupportingDocuments { get; set; }
}

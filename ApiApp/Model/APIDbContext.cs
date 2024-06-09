using System.Reflection.Metadata;
using ApiApp.Model;
using Microsoft.EntityFrameworkCore;


namespace Internship.Model
{
    public class APIDbContext: DbContext
    {
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<PersonDetails> PersonsDetails { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<Department> Departments { get; set; }

        public APIDbContext(DbContextOptions<APIDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>()
                .HasIndex(a => a.Username)
                .IsUnique();

            modelBuilder.Entity<PersonDetails>()
                .HasIndex(pd => pd.PersonId)
                .IsUnique();
        }
    }
}

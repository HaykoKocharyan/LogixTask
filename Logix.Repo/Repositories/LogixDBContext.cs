using Logix.Repo.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logix.Repo.Repositories
{
    public class LogixDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<UserClass> UserClasses { get; set; }

        public LogixDBContext()
        {

        }

        public LogixDBContext(DbContextOptions<LogixDBContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=Logix;Username=postgres;Password=Hhkk0407",
                b => b.MigrationsAssembly("Logix.Repo"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.DateOfBirth)
                    .HasColumnType("date"); // Define the type as 'date' in the database
            });
            modelBuilder.Entity<User>()
          .Property(u => u.FullName)
         .HasComputedColumnSql("\"FirstName\" || ' ' || \"LastName\"", stored: true);

            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            });

            modelBuilder.Entity<UserClass>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.ClassId });

                entity.HasOne(uc => uc.User)
                    .WithMany(u => u.UserClasses)
                    .HasForeignKey(uc => uc.UserId);

                entity.HasOne(uc => uc.Class)
                    .WithMany(c => c.UserClasses)
                    .HasForeignKey(uc => uc.ClassId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}

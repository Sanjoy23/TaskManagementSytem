using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Task entity
            modelBuilder.Entity<Models.TaskEntity>(entity =>
            {
                entity.HasKey(t => t.Id);

                // CreatedByUser relationship - explicitly NO ACTION
                entity.HasOne(t => t.CreatedByUser)
                      .WithMany(u => u.CreatedTasks)
                      .HasForeignKey(t => t.CreatedByUserId)
                      .OnDelete(DeleteBehavior.NoAction)
                      .HasConstraintName("FK_Tasks_Users_CreatedByUserId");

                // AssignedToUser relationship - explicitly NO ACTION  
                entity.HasOne(t => t.AssignedToUser)
                      .WithMany(u => u.AssignedTasks)
                      .HasForeignKey(t => t.AssignedToUserId)
                      .OnDelete(DeleteBehavior.NoAction)
                      .HasConstraintName("FK_Tasks_Users_AssignedToUserId");

                // Team relationship - NO ACTION to avoid any cascade conflicts
                entity.HasOne(t => t.Team)
                      .WithMany(team => team.Tasks)
                      .HasForeignKey(t => t.TeamId)
                      .OnDelete(DeleteBehavior.NoAction)
                      .HasConstraintName("FK_Tasks_Teams_TeamId");
            });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
    }
}

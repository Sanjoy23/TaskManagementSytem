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
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                // User-Role relationship
                entity.HasOne(u => u.Role)
                    .WithMany(r => r.Users)
                    .HasForeignKey(u => u.RoleId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Users_Roles_RoleId");

                entity.HasIndex(u => u.Email).IsUnique();
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.Id);
            });

            // Configure Task entity
            modelBuilder.Entity<TaskEntity>(entity =>
            {
                entity.HasKey(t => t.Id);

                // Status relationship
                entity.HasOne(t => t.Status)
                      .WithMany(s => s.Tasks)
                      .HasForeignKey(t => t.StatusId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_Tasks_TasksStatus_StatusId");

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
        public DbSet<Role> Roles { get; set; }
        public DbSet<TasksStatus> TaskStatuses { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
    }
}

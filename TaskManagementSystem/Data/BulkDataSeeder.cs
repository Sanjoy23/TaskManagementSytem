using Bogus;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Data
{
    public class BulkDataSeeder
    {
        private readonly AppDbContext _context;

        public BulkDataSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync(int teamsCount = 100, int usersCount = 5000, int tasksCount = 100000)
        {
            // --- Roles ---
            var roles = await _context.Roles.ToListAsync();
            if (!roles.Any())
            {
                roles = new List<Role>
        {
            new Role { Id = Guid.NewGuid().ToString(), RoleName = "Admin" },
            new Role { Id = Guid.NewGuid().ToString(), RoleName = "Manager" },
            new Role { Id = Guid.NewGuid().ToString(), RoleName = "Employee" }
        };
                await _context.BulkInsertAsync(roles);
            }

            // --- Teams ---
            if (!await _context.Teams.AnyAsync())
            {
                var teamFaker = new Faker<Team>()
                    .RuleFor(t => t.Id, f => Guid.NewGuid().ToString())
                    .RuleFor(t => t.Name, f => f.Company.CompanyName())
                    .RuleFor(t => t.Description, f => f.Lorem.Sentence());

                var teams = teamFaker.Generate(teamsCount);
                await _context.BulkInsertAsync(teams);
            }
            var teamsList = await _context.Teams.ToListAsync();

            // --- Users ---
            if (!await _context.Users.AnyAsync())
            {
                var userFaker = new Faker<User>()
                    .RuleFor(u => u.Id, f => Guid.NewGuid().ToString())
                    .RuleFor(u => u.FullName, f => f.Name.FullName())
                    .RuleFor(u => u.Email, f => f.Internet.Email().ToLower() + Guid.NewGuid().ToString("N").Substring(0, 6)) // ensures uniqueness
                    .RuleFor(u => u.Password, f => f.Internet.Password())
                    .RuleFor(u => u.RoleId, f => f.PickRandom(roles).Id);


                var users = userFaker.Generate(usersCount);
                await _context.BulkInsertAsync(users);
            }
            var usersList = await _context.Users.ToListAsync();

            // --- Statuses ---
            var statuses = await _context.TaskStatuses.ToListAsync();
            if (!statuses.Any())
            {
                statuses = new List<TasksStatus>
        {
            new TasksStatus { Id = Guid.NewGuid().ToString(), Name = "Todo" },
            new TasksStatus { Id = Guid.NewGuid().ToString(), Name = "In Progress" },
            new TasksStatus { Id = Guid.NewGuid().ToString(), Name = "Done" }
        };
                await _context.BulkInsertAsync(statuses);
            }

            // --- Tasks ---
            if (!await _context.Tasks.AnyAsync())
            {
                var taskFaker = new Faker<TaskEntity>()
                    .RuleFor(t => t.Id, f => Guid.NewGuid().ToString())
                    .RuleFor(t => t.Title, f => f.Lorem.Sentence(4))
                    .RuleFor(t => t.Description, f => f.Lorem.Paragraph())
                    .RuleFor(t => t.StatusId, f => f.PickRandom(statuses).Id)
                    .RuleFor(t => t.AssignedToUserId, f => f.PickRandom(usersList).Id)
                    .RuleFor(t => t.CreatedByUserId, f => f.PickRandom(usersList).Id)
                    .RuleFor(t => t.TeamId, f => f.PickRandom(teamsList).Id)
                    .RuleFor(t => t.DueDate, f => f.Date.Future());

                var tasks = taskFaker.Generate(tasksCount);

                const int batchSize = 20000;
                for (int i = 0; i < tasks.Count; i += batchSize)
                {
                    var batch = tasks.Skip(i).Take(batchSize).ToList();
                    await _context.BulkInsertAsync(batch);
                }
            }
        }

    }
}

using Domain.Entities;
using Domain.Interface;
using Infrastruture.Data;

namespace Infrastruture.Repository
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

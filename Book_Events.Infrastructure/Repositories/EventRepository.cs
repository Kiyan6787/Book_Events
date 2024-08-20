using Book_Events.Infrastructure.Context;
using Book_Events.Infrastructure.Context.Entities;
using Book_Events.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Events.Infrastructure.Repositories
{
    public class EventRepository : Repository<BookEvent>, IEventRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<BookEvent>> GetUserEventsAsync(string email)
        {
            return await _context.BookEvent.AsNoTracking().Where(x => x.Email == email).OrderBy(x => x.Date).ToListAsync();
        }

        public async Task<List<BookEvent>> GetPastPublicEventsAsync()
        {
            return await _context.BookEvent.AsNoTracking().Where(x => x.Date < DateOnly.FromDateTime(DateTime.Now) && x.Type == EventType.Public).ToListAsync();
        }

        public async Task<List<BookEvent>> GetUpcomingPublicEventsAsync()
        {
            return await _context.BookEvent.AsNoTracking().Where(x => x.Date >= DateOnly.FromDateTime(DateTime.Now) && x.Type == EventType.Public).ToListAsync();
        }

        public async Task<BookEvent> GetDetailsWithComments(int id)
        {
            return await _context.BookEvent.Include(x => x.Comments).FirstOrDefaultAsync(x => x.Id == id);
        }

    }
}

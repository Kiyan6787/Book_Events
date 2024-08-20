using Book_Events.Infrastructure.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Events.Infrastructure.Interfaces
{
    public interface IEventRepository : IRepository<BookEvent>
    {
        Task<List<BookEvent>> GetUserEventsAsync(string email);
        Task<List<BookEvent>> GetPastPublicEventsAsync();
        Task<List<BookEvent>> GetUpcomingPublicEventsAsync();
        Task<BookEvent> GetDetailsWithComments(int id);
    }
}

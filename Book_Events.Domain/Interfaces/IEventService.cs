using Book_Events.Domain.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Events.Domain.Interfaces
{
    public interface IEventService
    {
        Task<BookEventDTO?> GetEventByIdAsync(int id);
        Task<bool> CreateEventAsync(BookEventDTO bookEvent, string email);
        Task<bool> UpdateEventAsync(BookEventDTO bookEvent, int id);
        Task<bool> DeleteEventAsync(int id);
        Task<List<BookEventDTO>> GetUserEventsAsync(string email);
        Task<List<BookEventDTO>> GetPastPublicEventsAsync();
        Task<List<BookEventDTO>> GetUpcomingPublicEventsAsync();
        Task<bool> AddCommentAsync(int eventId, string commentText, string email);
        Task<BookEventDTO> GetDetailsWithComments(int id);
        Task<List<BookEventDTO>> EventsInvitedToo(string email);
    }
}

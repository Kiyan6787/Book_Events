using Book_Events.Domain.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Events.Domain.Interfaces
{
    public interface IFacade
    {
        Task<IEnumerable<BookEventDTO>> GetAllEventsAsync();
        Task<BookEventDTO> GetEventByIdAsync(int id);
        Task<bool> UpdateEventAsync(BookEventDTO eventDTO, int id);
        Task<bool> CreateEventAsync(BookEventDTO eventDTO, string email);
        Task<bool> DeleteEventAsync(int id);
        Task<bool> AddCommentAsync(int eventId, string email, string commentText);
        Task<IEnumerable<BookEventDTO>> GetUserEventsAsync(string email);
        Task<IEnumerable<BookEventDTO>> GetEventsInvitedToAsync(string email);
        Task<List<BookEventDTO>> GetPastPublicEventsAsync();
        Task<List<BookEventDTO>> GetUpcomingPublicEventsAsync();
        Task<BookEventDTO> GetDetailsWithComments(int id);
    }
}

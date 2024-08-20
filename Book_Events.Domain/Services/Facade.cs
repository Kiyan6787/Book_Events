using Book_Events.Domain.DTOS;
using Book_Events.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Events.Domain.Services
{
    public class Facade : IFacade
    {
        private readonly IEventService _eventService;
        private readonly IAdminService _adminService;

        public Facade(IEventService eventService, IAdminService adminService)
        {
            _eventService = eventService;
            _adminService = adminService;
        }

        public async Task<bool> AddCommentAsync(int eventId, string email, string commentText)
        {
            return await _eventService.AddCommentAsync(eventId, email, commentText);
        }

        public async Task<bool> CreateEventAsync(BookEventDTO eventDTO, string email)
        {
            return await _eventService.CreateEventAsync(eventDTO, email);
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            return await _eventService.DeleteEventAsync(id);
        }

        public async Task<IEnumerable<BookEventDTO>> GetAllEventsAsync()
        {
            return await _adminService.GetAllEventsAsync();
        }

        public async Task<BookEventDTO> GetDetailsWithComments(int id)
        {
            return await _eventService.GetDetailsWithComments(id);
        }

        public async Task<BookEventDTO> GetEventByIdAsync(int id)
        {
            return await _eventService.GetEventByIdAsync(id);
        }

        public async Task<IEnumerable<BookEventDTO>> GetEventsInvitedToAsync(string email)
        {
            return await _eventService.EventsInvitedToo(email);
        }

        public async Task<List<BookEventDTO>> GetPastPublicEventsAsync()
        {
            return await _eventService.GetPastPublicEventsAsync();
        }

        public async Task<List<BookEventDTO>> GetUpcomingPublicEventsAsync()
        {
            return await _eventService.GetUpcomingPublicEventsAsync();
        }

        public async Task<IEnumerable<BookEventDTO>> GetUserEventsAsync(string email)
        {
            return await _eventService.GetUserEventsAsync(email);
        }

        public async Task<bool> UpdateEventAsync(BookEventDTO eventDTO, int id)
        {
            return await _eventService.UpdateEventAsync(eventDTO, id);
        }
    }
}

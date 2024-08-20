using Book_Events.Domain.DTOS;
using Book_Events.Infrastructure.Context.Entities;

namespace Book_Events.ViewModels
{
    public class HomePageViewModel
    {
        public List<BookEventDTO> PublicUpcomingEvents { get; set; }
        public List<BookEventDTO> PublicPastEvents { get; set; }
    }
}

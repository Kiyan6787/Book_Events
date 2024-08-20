using Book_Events.Domain.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Events.Domain.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<BookEventDTO>> GetAllEventsAsync();
        Task<bool> UpdateEvent(BookEventDTO eventDTO, int id);
    }
}

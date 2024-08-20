using AutoMapper;
using Book_Events.Domain.DTOS;
using Book_Events.Domain.Interfaces;
using Book_Events.Infrastructure.Context.Entities;
using Book_Events.Infrastructure.Interfaces;
using Book_Events.Infrastructure.Unit_of_Work;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Events.Domain.Services
{
    public class AdminServices : IAdminService
    {
        private readonly IMapper _mapper;
        private readonly IEventRepository _eventRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdminServices(IMapper mapper, IEventRepository eventRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _eventRepository = eventRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<BookEventDTO>> GetAllEventsAsync()
        {
            var result = await _eventRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<BookEventDTO>>(result);
        }

        public async Task<bool> UpdateEvent(BookEventDTO eventDTO, int id)
        {
            eventDTO.Id = id;
            var map = _mapper.Map<BookEvent>(eventDTO);
            var res = await _unitOfWork.BookRepo.UpdateAsync(map);
            await _unitOfWork.Save();

            return res;
        }
    }
}

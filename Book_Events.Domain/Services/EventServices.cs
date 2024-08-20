using AutoMapper;
using Book_Events.Domain.DTOS;
using Book_Events.Domain.ExceptionHandler;
using Book_Events.Domain.Interfaces;
using Book_Events.Infrastructure.Context.Entities;
using Book_Events.Infrastructure.Interfaces;
using Book_Events.Infrastructure.Unit_of_Work;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Book_Events.Domain.Services
{
    public class EventServices : IEventService
    {
        private readonly IMapper _mapper;
        private readonly IEventRepository _eventRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EventServices(IMapper mapper, IEventRepository eventRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _eventRepository = eventRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the event with the matching Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CustomExceptionHandler]
        public async Task<BookEventDTO?> GetEventByIdAsync(int id)
        {
            var res = await _unitOfWork.BookRepo.GetByIdAsync(id);

            if (res == null)
            {
                throw new ArgumentException($"Event with Id: {id} not found");
            }

            return _mapper.Map<BookEventDTO>(res);
        }

        /// <summary>
        /// Creates an event
        /// </summary>
        /// <param name="createViewModel"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> CreateEventAsync(BookEventDTO createViewModel, string email)
        {
            var newEvent = _mapper.Map<BookEvent>(createViewModel);

            var res = await _unitOfWork.BookRepo.InsertAsync(newEvent);
            await _unitOfWork.Save();

            return res;
        }

        /// <summary>
        /// Updates an event
        /// </summary>
        /// <param name="bookEventDTO"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<bool> UpdateEventAsync(BookEventDTO bookEventDTO, int Id)
        {
                bookEventDTO.Id = Id;
                var map = _mapper.Map<BookEvent>(bookEventDTO);
                var res = await _unitOfWork.BookRepo.UpdateAsync(map);
                await _unitOfWork.Save();

                return res;
        }

        /// <summary>
        /// Deletes an event
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteEventAsync(int id)
        {
            //var product = await _eventRepository.GetByIdAsync(id);
            var prod = await _unitOfWork.BookRepo.GetByIdAsync(id);

            if (prod is null)
                return false;

            //var result = await _eventRepository.DeleteAsync(product);
            var res = await _unitOfWork.BookRepo.DeleteAsync(prod);
            await _unitOfWork.Save();

            return res;
        }

        /// <summary>
        /// Gets a list of the logged in users events
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<List<BookEventDTO>> GetUserEventsAsync(string email)
        {
            var events = await _eventRepository.GetUserEventsAsync(email);
            return _mapper.Map<List<BookEventDTO>>(events);
        }

        /// <summary>
        /// Gets a list if the past events.
        /// </summary>
        /// <returns></returns>
        public async Task<List<BookEventDTO>> GetPastPublicEventsAsync()
        {
            var events = await _eventRepository.GetPastPublicEventsAsync();
            return _mapper.Map<List<BookEventDTO>>(events);
        }

        /// <summary>
        /// Gets a list of the upcoming events
        /// </summary>
        /// <returns></returns>
        public async Task<List<BookEventDTO>> GetUpcomingPublicEventsAsync()
        {
            var events = await _eventRepository.GetUpcomingPublicEventsAsync();
            return _mapper.Map<List<BookEventDTO>>(events);
        }

        /// <summary>
        /// Allows user to post a comment
        /// </summary>
        /// <param name="bookEventId"></param>
        /// <param name="email"></param>
        /// <param name="commentText"></param>
        /// <returns></returns>
        public async Task<bool> AddCommentAsync(int bookEventId, string email, string commentText)
        {
            var bookEvent = await _unitOfWork.BookRepo.GetByIdAsync(bookEventId);

            if (bookEvent == null)
            {
                return false;
            }

            var newComment = new Comment
            {
                Email = email,
                Text = commentText,
                CreatedAt = DateTime.Now,
                BookEventId = bookEventId
            };

            await _unitOfWork.CommentRepo.InsertAsync(newComment);
            await _unitOfWork.Save();

            return true; 
        }

        /// <summary>
        /// Gets the details of an event along with the comments
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CustomExceptionHandler]
        public async Task<BookEventDTO> GetDetailsWithComments(int id)
        {
            var result = await _eventRepository.GetDetailsWithComments(id);

            if (result == null)
            {
                throw new ArgumentException($"Event with Id: {id} not found");
            }

            return _mapper.Map<BookEventDTO>(result);
        }

        /// <summary>
        /// Gets a list of events that a user has been invited too.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        public async Task<List<BookEventDTO>> EventsInvitedToo(string userEmail)
        {
            var allEvents = await _eventRepository.GetAllAsync();

            var userEvents = allEvents.Where(e => e.Invites?.Split(',').Select(email => email.Trim()).Contains(userEmail) == true).ToList();
            return userEvents.Select(e => _mapper.Map<BookEventDTO>(e)).ToList();
        }
    }
}

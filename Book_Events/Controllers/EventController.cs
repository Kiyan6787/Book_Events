using AutoMapper;
using Book_Events.Domain.DTOS;
using Book_Events.Domain.Factory;
using Book_Events.Domain.Interfaces;
using Book_Events.Domain.Services;
using Book_Events.Infrastructure.Context;
using Book_Events.Plugins.Logging;
using Book_Events.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Security.Claims;

namespace Book_Events.Controllers
{
    public class EventController : Controller
    {
        private readonly IFacade _facade;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILog _ILog;

        public EventController(UserManager<AppUser> userManager, IFacadeFactory<IFacade> eventFactory)
        {
            _facade = eventFactory.GetFacade(this._facade);
            _userManager = userManager;
            _ILog = Log.Instance();
        }

        /// <summary>
        /// Gets a list of events that the logged in user created.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetUserEvents")]
        public async Task<IActionResult> GetUserEvents()
        {

            try
            {
                //Get logged in users email
                var user = await _userManager.GetUserAsync(User);
                var email = user?.Email;

                //Get their events
                var events = await _facade.GetUserEventsAsync(email);
                _ILog.LogInformation("GetUserEvents log");

                return View(events);
            }
            catch(Exception ex)
            {
                _ILog.LogException("GetUserEvents error: " + ex.Message);

                return BadRequest("Unable to get user events. Try again later");
            }

        }

        /// <summary>
        /// Gets an event based on the ID parameter.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetProductById/{id:int}")]
        [ProducesResponseType(typeof(BookEventDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<BookEventDTO>> GetProductById(int id)
        {
            //try
            //{
                //Get the details of an event along with the comments.
                var result = await _facade.GetDetailsWithComments(id);

                if (result is not null) return View(result);

                _ILog.LogException($"Event with Id: {id} not found");
                return NotFound($"Event with {id} not found");
            //}
            //catch (Exception ex)
            //{
            //    _ILog.LogException("GetEventById error: " + ex);

            //    return BadRequest("Unable to event. Try again later");
            //}
        }

        /// <summary>
        /// Returns the create view.
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            //Check if model state is valid and return the relevanr view
            if (ModelState.IsValid)
            {
                var viewModel = new BookEventDTO();
                return View(viewModel);
            }
            else
            {
                return BadRequest("Something went wrong");
            }
            
        }

        /// <summary>
        /// Action method for posting data to the database to create a new event.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost("CreateProduct"), ValidateAntiForgeryToken]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Authorize]
        public async Task<IActionResult> CreateEvent(BookEventDTO bookEvent)
        {
            if (!ModelState.IsValid) return BadRequest("Model is not valid");

            //Get logged in user email.
            var user = await _userManager.GetUserAsync(User);
            var email = user?.Email;

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); 
                }

                //Call the create method.
                var createdProduct = await _facade.CreateEventAsync(bookEvent,email);

                //If successful redirect to the logged in users events
                if (createdProduct)
                {
                    return RedirectToAction("GetUserEvents", "Event");
                }
                else
                {
                    return BadRequest("Error creating event");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                _ILog.LogException($"Error creating event, error message: {ex.Message}");

                // Return a server error response
                return StatusCode(500, "Error creating event");
            }
        }

        /// <summary>
        /// Returns the edit view.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int id)
        {
            if (!ModelState.IsValid) return BadRequest("Model is not valid");

            try
            {
                //Get the event by the id and returns the relevate view with the data.
                var events = await _facade.GetEventByIdAsync(id);

                return View(events);
            }
            catch (Exception ex)
            {
                _ILog.LogException($"Cannot get event, error message: {ex.Message}: ");

                return BadRequest("Cannot get event");
            }
            
        }

        /// <summary>
        /// Action method for updating an event.
        /// </summary>
        /// <param name="bookEvent"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateEvent(BookEventDTO bookEvent)
        {
            //Checks if the model state is valid, if not prints errors.
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _ILog.LogException("Model state error: {ErrorMessage}" + error.ErrorMessage);
                    }
                }
                return BadRequest("Model not valid");
            }

            try
            {
                //Gets logged in users email.
                var user = await _userManager.GetUserAsync(User);
                var email = user?.Email;

                //Calls update event service.
                var result = await _facade.UpdateEventAsync(bookEvent, bookEvent.Id);
                _ILog.LogException(result.ToString());

                //if successful, redirects to event list.
                if (result)
                {
                    return RedirectToAction("GetUserEvents");
                }

                _ILog.LogException("Event not updated");
                return BadRequest("Error updating event");
            }
            catch (Exception ex)
            {
                _ILog.LogException($"An error occurred while updating the event, error: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Action method for deleting an event.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult<BookEventDTO>> DeleteEvent(int id)
        {
            if (!ModelState.IsValid) return BadRequest("Model is not valid");

            //Calls delete service.
            var result = await _facade.DeleteEventAsync(id);

            //if successful, redirects to user events.
            if (result) return RedirectToAction("GetUserEvents");

            _ILog.LogException($"Event with Id: {id} not remove");
            return BadRequest($"Event with {id} not remove");
        }

        /// <summary>
        /// Displays past and upcoming events on the home page.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> HomePage()
        {
            var user = await _userManager.GetUserAsync(User);

            try
            {
                //Checks whether the user is an admin and redirects to a different view based on that.
                if (user != null && await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return RedirectToAction("GetAllEvents", "Admin");
                }
                else
                {
                    var events = new HomePageViewModel
                    {
                        PublicUpcomingEvents = await _facade.GetUpcomingPublicEventsAsync(),
                        PublicPastEvents = await _facade.GetPastPublicEventsAsync()
                    };

                    return View("HomePage", events);
                }
            }
            catch(Exception ex)
            {
                _ILog.LogException("HomePage error: " + ex.Message);

                return BadRequest("Cannot get home page. Try again later.");
            }
        }

        /// <summary>
        /// Returns a view containg events that a user has been invited too.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> EventsInvitedToo()
        {
            try
            {
                //Gets logged in users email
                var user = await _userManager.GetUserAsync(User);
                var email = user?.Email;

                //Calls events invited too service.
                var events = await _facade.GetEventsInvitedToAsync(email);

                return View(events);
            }
            catch (Exception ex)
            {
                _ILog.LogException("EventsInvitedToo error: " + ex.Message);
                return BadRequest("Cannot get events invited too. Try again later");
            }
            
        }

        /// <summary>
        /// Returns a view containing the details of an event. This is just for individual users.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult<BookEventDTO>> Details(int id)
        {
            //Calls GetDetails service to get the details of an event.
            var result = await _facade.GetEventByIdAsync(id);

            if (result is not null) return View(result);

            _ILog.LogException($"Event with Id {id} not found");
            return NotFound($"Event with {id} not found");
        }

        /// <summary>
        /// Action method that allows users to post a comment under an event.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="commentText"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddComment(int eventId, string commentText)
        {
            //Gets logged in users email.
            var user = await _userManager.GetUserAsync(User);
            var email = user?.Email;

            //Prints model state errors.
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _ILog.LogException("Model state error: {ErrorMessage}: " + error.ErrorMessage);
                    }
                }
                return BadRequest("Model not valid");
            }

            try
            {
                //Calls AddComment service
                var result = await _facade.AddCommentAsync(eventId, email, commentText);

                //If successful, redirect to details page.
                if (result)
                {
                    return RedirectToAction("GetProductById", new { id = eventId });
                }
                else
                {
                    return BadRequest("Comment failed");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Comment failed");
            }

        }

        public async Task<IActionResult> Error()
        {
            return View();
        }
    }
}

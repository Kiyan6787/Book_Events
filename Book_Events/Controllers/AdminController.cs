using AutoMapper;
using Book_Events.Attributes;
using Book_Events.Domain.DTOS;
using Book_Events.Domain.Factory;
using Book_Events.Domain.Interfaces;
using Book_Events.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Book_Events.Controllers
{
    //[Authorize(Roles ="Admin")]
    [AccessControl(RoleGroup = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFacade _facade;

        public AdminController(ILogger<EventController> logger, UserManager<AppUser> userManager, IFacadeFactory<IFacade> factory)
        {
            _facade = factory.GetFacade(this._facade);
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _facade.GetAllEventsAsync();

            return View(events);
        }

        public async Task<ActionResult> Edit(int id)
        {
            if (!ModelState.IsValid) return BadRequest("Model is not valid");

            var events = await _facade.GetEventByIdAsync(id);

            return View(events);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateEvent(BookEventDTO bookEvent)
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _logger.LogError("Model state error: {ErrorMessage}", error.ErrorMessage);
                    }
                }
                return BadRequest("Model not valid");
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                var email = user?.Email;

                var result = await _facade.UpdateEventAsync(bookEvent, bookEvent.Id);
                _logger.LogError(result.ToString());

                if (result)
                {
                    return RedirectToAction("GetAllEvents");
                }

                _logger.LogInformation("Event not updated");
                return BadRequest("Error updating event");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the event");
                return StatusCode(500, "Internal server error");
            }
        }

        public async Task<ActionResult<BookEventDTO>> Details(int id)
        {
            var result = await _facade.GetEventByIdAsync(id);

            if (result is not null) return View(result);

            _logger.LogInformation("Event with id {Id} not found", id);
            return NotFound($"Event with {id} not found");
        }

        public IActionResult ErrorView()
        {
            return View();
        }
    }
}

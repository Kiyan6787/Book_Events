using Book_Events.Infrastructure.Context.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Book_Events.ViewModels
{
    public class EditViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateOnly Date { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Starting time is required")]
        public string StartTime { get; set; } = "00:00";

        [Required]
        public EventType Type { get; set; } = EventType.Public;

        [Range(0, 4, ErrorMessage = "Event can not be longer than 4 hours")]
        public int? Duration { get; set; }

        [StringLength(50, ErrorMessage = "Description cannot be longer than 50 characters")]
        public string? Description { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string? OtherDetails { get; set; }

        public string? InviteByEmail { get; set; }
        public List<SelectListItem> TimeOptions { get; set; }

        public EditViewModel()
        {
            TimeOptions = Enumerable.Range(0, 24)
                          .Select(x => new SelectListItem
                          {
                              Value = TimeSpan.FromHours(x).ToString(@"hh\:mm"),
                              Text = TimeSpan.FromHours(x).ToString(@"hh\:mm")
                          })
                          .ToList();
        }
    }
}

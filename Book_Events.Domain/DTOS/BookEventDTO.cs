using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Book_Events.Infrastructure.Context.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using FluentValidation;

namespace Book_Events.Domain.DTOS
{
    public class BookEventDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateOnly Date { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        public string StartTime { get; set; } 

        [Required]
        public EventType Type { get; set; } = EventType.Public;

        [Range(0, 4, ErrorMessage = "Event can not be longer than 4 hours")]
        public int? Duration { get; set; }

        [StringLength(50, ErrorMessage = "Description cannot be longer than 50 characters")]
        public string? Description { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string? OtherDetails { get; set; }

        public string? Invites { get; set; }
        public List<SelectListItem> TimeOptions { get; set; }

        public List<Comment>? Comments { get; set; } = new List<Comment>();

        public BookEventDTO()
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
    public class BookEventDTOModelValidator : AbstractValidator<BookEventDTO>
    {
        public BookEventDTOModelValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
            RuleFor(x => x.Date).NotEmpty().WithMessage("Date is required");
            RuleFor(x => x.StartTime).NotEmpty().WithMessage("Start time is required");
            RuleFor(x => x.Location).NotEmpty().WithMessage("Location is required FV");
        }
    }
}

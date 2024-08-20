using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Book_Events.Infrastructure.Context.Entities
{
    public class BookEvent : BaseEntity
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string StartTime { get; set; } = "00:00";

        [Required]
        public EventType Type { get; set; } = EventType.Public;
        public int? Duration { get; set; }

        [MaxLength(50)]
        public string? Description { get; set; }

        [MaxLength(500)]
        public string? OtherDetails { get; set; }
        public string? Invites { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }

}

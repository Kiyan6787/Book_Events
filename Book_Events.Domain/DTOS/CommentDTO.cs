using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Events.Domain.DTOS
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public int BookEventId { get; set; }
    }
}

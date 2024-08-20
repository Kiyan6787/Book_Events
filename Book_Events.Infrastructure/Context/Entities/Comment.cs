using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Events.Infrastructure.Context.Entities
{
    public class Comment : BaseEntity
    {
        public string Email { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public int BookEventId { get; set; }
        public bool IsRead { get; set; } = false;
        public BookEvent BookEvent { get; set; }
    }
}

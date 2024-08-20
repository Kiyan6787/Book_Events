using Book_Events.Infrastructure.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Events.Infrastructure.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
    }
}

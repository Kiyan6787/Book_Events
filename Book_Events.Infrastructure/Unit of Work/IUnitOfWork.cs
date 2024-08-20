using Book_Events.Infrastructure.Context.Entities;
using Book_Events.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Events.Infrastructure.Unit_of_Work
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<BookEvent> BookRepo { get; }
        IRepository<Comment> CommentRepo { get; }
        Task Save();
    }
}

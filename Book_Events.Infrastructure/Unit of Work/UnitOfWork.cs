using Book_Events.Infrastructure.Context;
using Book_Events.Infrastructure.Context.Entities;
using Book_Events.Infrastructure.Interfaces;
using Book_Events.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Events.Infrastructure.Unit_of_Work
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;
        public IRepository<BookEvent> bookRepo;
        public IRepository<Comment> commentRepo;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<BookEvent> BookRepo
        {
            get
            {
                if (bookRepo == null)
                {
                    bookRepo = new Repository<BookEvent>(_context);
                }
                return bookRepo;
            }
        }

        public IRepository<Comment> CommentRepo
        {
            get
            {
                if (commentRepo == null)
                {
                    commentRepo = new Repository<Comment>(_context);
                }
                return commentRepo;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_context != null)
                    {
                        _context.Dispose();
                    }
                }
            }
            _disposed = true;
        }
    }
}

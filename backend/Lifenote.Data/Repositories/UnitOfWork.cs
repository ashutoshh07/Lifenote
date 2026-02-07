using Lifenote.Core.Interfaces;
using Lifenote.Data.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lifenote.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LifenoteDbContext _context;
        public IUserInfoRepository Users { get; private set; }
        public INoteRepository Notes { get; private set; }

        public UnitOfWork(LifenoteDbContext context)
        {
            _context = context;
            Users = new UserInfoRepository(_context);
            Notes = new NoteRepository(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

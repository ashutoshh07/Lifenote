using Lifenote.Core.Interfaces;
using Lifenote.Data.Data;

namespace Lifenote.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LifenoteDbContext _context;
        public IUserInfoRepository Users { get; private set; }
        public INoteRepository Notes { get; private set; }
        public IHabitRepository Habits { get; private set; }

        public UnitOfWork(LifenoteDbContext context)
        {
            _context = context;
            Users = new UserInfoRepository(_context);
            Notes = new NoteRepository(_context);
            Habits = new HabitRepository(_context);
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

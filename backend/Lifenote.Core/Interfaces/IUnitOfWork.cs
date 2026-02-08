namespace Lifenote.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserInfoRepository Users { get; }
        INoteRepository Notes { get; }
        IHabitRepository Habits { get; }
        Task<int> SaveChangesAsync();
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Lifenote.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserInfoRepository Users { get; }
        INoteRepository Notes { get; }
        Task<int> SaveChangesAsync();
    }
}

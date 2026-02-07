using Lifenote.Core.Models;

namespace Lifenote.Core.Interfaces
{
    public interface IUserInfoRepository
    {
        Task<UserInfo> GetByIdAsync(int id);
        Task<UserInfo> GetByAuthProviderIdAsync(string authProviderId);
        Task<UserInfo> GetByUsernameAsync(string username);
        Task<UserInfo> GetByEmailAsync(string email);

        Task<bool> IsUsernameAvailableAsync(string username);
        Task<bool> IsEmailAvailableAsync(string email);

        Task AddAsync(UserInfo user);
        void Update(UserInfo user);
    }

}

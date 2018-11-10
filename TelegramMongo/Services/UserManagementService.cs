using System.Linq;
using System.Threading.Tasks;
using TelegramMongo.Models;
using TelegramMongo.Repository;

namespace TelegramMongo.Services
{
    public class UserManagementService
    {
        private readonly GenericRepository<User> _userStorage;

        public UserManagementService()
        {
            _userStorage = new GenericRepository<User>();
        }

        public async Task<User> GetUserAsync(long chatId)
        {
            return (await _userStorage.ReadAsync(p => p.ChatId == chatId)).SingleOrDefault();
        }

        public async Task<User> SignUpUserAsync(UserRegistrationModel model)
        {
            await _userStorage.AddAsync(
                new User
                {
                    ChatId = model.ChatId,
                    LastName = model.LastName,
                    FirstName = model.FirstName,
                    BooksGot = 0
                });

            return (await _userStorage.ReadAsync(p => p.ChatId == model.ChatId)).Single();
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userStorage.ReplaceAsync(q => q.ChatId == user.ChatId, user);
        }
    }
}
using DDD.Models.Data;
using DDD.Models.Models;
using MongoDB.Driver;

namespace DDD.Models.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context) => _context = context;

        public async Task<User> GetUserAsync(string telegramId)
            => await _context.Users.Find(u => u.TelegramId == telegramId).FirstOrDefaultAsync();

        public async Task AddUserAsync(User user)
            => await _context.Users.InsertOneAsync(user);

        public async Task UpdateUserAsync(User user)
            => await _context.Users.ReplaceOneAsync(u => u.Id == user.Id, user);

        public async Task<List<User>> GetUsersAsync()
            => await _context.Users.Find(_ => true).ToListAsync();
    }
}

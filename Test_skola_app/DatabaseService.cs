using SQLite;
using Test_skola_app.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test_skola_app.Data;

namespace Test_skola_app
{
    public class DatabaseService
    {
        private readonly ApplicationDbContext _dbContext;

        public DatabaseService(string dbPath)
        {
            _dbContext = new ApplicationDbContext(dbPath);
        }

        // User operations
        public Task<List<User>> GetUsersAsync()
            => _dbContext.GetUsersAsync();

        public Task<int> SaveUserAsync(User user)
            => _dbContext.SaveUserAsync(user);

        public Task<int> DeleteUserAsync(User user)
            => _dbContext._database.DeleteAsync(user);

        // Extend with additional operations for other models as needed
    }

    public class ApplicationDbContext
    {
        public readonly SQLiteAsyncConnection _database;

        public ApplicationDbContext(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            InitializeAsync().Wait();
        }

        private async Task InitializeAsync()
        {
            await _database.CreateTableAsync<User>();
            await _database.CreateTableAsync<UserPrivileges>();
            await _database.CreateTableAsync<Chat>();
            await _database.CreateTableAsync<Message>();
            await _database.CreateTableAsync<ChatUser>();
        }

        public Task<List<User>> GetUsersAsync() => _database.Table<User>().ToListAsync();
        public Task<int> SaveUserAsync(User user) => _database.InsertAsync(user);
    }
}
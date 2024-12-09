using SQLite;
using Test_skola_app.Models;

namespace Test_skola_app.Data
{
    public class ApplicationDbContext
    {
        private readonly SQLiteAsyncConnection _database;

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

        // User operations
        public Task<List<User>> GetUsersAsync()
            => _database.Table<User>().ToListAsync();

        public Task<int> SaveUserAsync(User user)
            => _database.InsertAsync(user);

        // UserPrivileges operations
        public Task<List<UserPrivileges>> GetUserPrivilegesAsync()
            => _database.Table<UserPrivileges>().ToListAsync();

        public Task<int> SaveUserPrivilegesAsync(UserPrivileges privileges)
            => _database.InsertAsync(privileges);

        // Chat operations
        public Task<List<Chat>> GetChatsAsync()
            => _database.Table<Chat>().ToListAsync();

        public Task<int> SaveChatAsync(Chat chat)
            => _database.InsertAsync(chat);

        // Message operations
        public Task<List<Message>> GetMessagesAsync()
            => _database.Table<Message>().ToListAsync();

        public Task<List<Message>> GetChatMessagesAsync(int chatId)
            => _database.Table<Message>().Where(m => m.ChatId == chatId).ToListAsync();

        public Task<int> SaveMessageAsync(Message message)
            => _database.InsertAsync(message);

        // ChatUser operations
        public Task<List<ChatUser>> GetChatUsersAsync()
            => _database.Table<ChatUser>().ToListAsync();

        public Task<int> SaveChatUserAsync(ChatUser chatUser)
            => _database.InsertAsync(chatUser);
    }
}
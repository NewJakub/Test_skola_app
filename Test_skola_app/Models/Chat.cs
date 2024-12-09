using SQLite;

namespace Test_skola_app.Models
{
    [Table("Chats")]
    public class Chat
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        [Ignore]
        public List<Message> Messages { get; set; }

        [Ignore]
        public List<ChatUser> Users { get; set; }
    }
}
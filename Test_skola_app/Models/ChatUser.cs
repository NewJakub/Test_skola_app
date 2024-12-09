using SQLite;

namespace Test_skola_app.Models
{
    [Table("ChatUsers")]
    public class ChatUser
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int ChatId { get; set; }

        [Indexed]
        public int UserId { get; set; }
    }
}
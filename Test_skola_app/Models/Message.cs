using SQLite;

namespace Test_skola_app.Models
{
    [Table("Messages")]
    public class Message
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int ChatId { get; set; }

        [Indexed]
        public int SenderId { get; set; }

        public string Content { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
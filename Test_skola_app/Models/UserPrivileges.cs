using SQLite;

namespace Test_skola_app.Models
{
    [Table("UserPrivileges")]
    public class UserPrivileges
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int UserId { get; set; }

        public bool CanMessageAdmin { get; set; }
        public bool CanMessageStudent { get; set; }
        public bool CanMessageParent { get; set; }
        public bool CanMessageTeacher { get; set; }
        public bool CanCreateGroups { get; set; }
        public bool CanAssignPrivileges { get; set; }
    }
}
namespace ItAcademy.Models
{
    public class GroupStudent
    {

        public int Id { get; set; }
        public Students Students { get; set; }
        public int StudentsId { get; set; }
        public Groups Groups { get; set; }
        public int GroupsId { get; set; }
    }
}

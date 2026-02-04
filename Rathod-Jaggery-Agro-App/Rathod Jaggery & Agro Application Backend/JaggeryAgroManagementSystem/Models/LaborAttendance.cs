namespace JaggeryAgroManagementSystem.Models
{
    public class LaborAttendance
    {
        public int Id { get; set; }
        public int LaborId { get; set; }
        public Labor Labor { get; set; }

        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
    }

}

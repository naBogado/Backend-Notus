namespace Notus.Models.Exam
{
    public class Exam
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime Date { get; set; }
        public int ClassId { get; set; }
        public float Grade { get; set; }
    }
}

namespace Notus.Models.Event
{
    public class Event
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}
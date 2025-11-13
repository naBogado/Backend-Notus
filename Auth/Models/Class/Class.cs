namespace Notus.Models.Class
{
    public class Class
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int? ProfesorId { get; set; }
    }
}
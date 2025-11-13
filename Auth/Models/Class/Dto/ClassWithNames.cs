namespace Notus.Models.Class.Dto
{
    public class ClassWithNames
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int ProfesorId { get; set; }
        public List<int> EstudiantesIds { get; set; } = new();
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notus.Models.Class
{
    public class Class
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int ProfessorId { get; set; }
        // LISTA DE ESTUDIANTES INSCRITOS EN LA CLASE
        // DEBERIA SER TIPO USER O TIPO STUDENT?
        public List<int> StudentId { get; set; } = new();
    }
}

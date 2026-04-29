using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningKidsAPI.Models
{
    public class ChatHistorial
    {
        [Key]
        public int idChat { get; set; }

        public int? idAlumno { get; set; }

        public string? mensaje { get; set; }

        public string? respuesta { get; set; }

        public DateTime? fecha { get; set; } = DateTime.UtcNow;

        [ForeignKey("idAlumno")]
        public Alumno? Alumno { get; set; }
    }
}
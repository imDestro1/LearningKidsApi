using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningKidsAPI.Models
{
    public class Resultado
    {
        [Key]
        public int idResultado { get; set; }

        public int? idAlumno { get; set; }

        public int? idPrueba { get; set; }

        public decimal? calificacion { get; set; }

        public DateTime? fecha { get; set; } = DateTime.UtcNow;

        [ForeignKey("idAlumno")]
        public Alumno? Alumno { get; set; }

        [ForeignKey("idPrueba")]
        public Prueba? Prueba { get; set; }
    }
}
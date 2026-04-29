using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningKidsAPI.Models
{
    public class Alumno
    {
        [Key]
        [ForeignKey("Usuario")]
        public int idAlumno { get; set; }

        public int? idTutor { get; set; }

        public int? grado { get; set; }

        [ForeignKey("idTutor")]
        public Tutor? Tutor { get; set; }

        public Usuario? Usuario { get; set; }

        public ICollection<DocenteAlumno> DocenteAlumnos { get; set; } = new List<DocenteAlumno>();
        public ICollection<Resultado> Resultados { get; set; } = new List<Resultado>();
        public ICollection<ChatHistorial> ChatHistorial { get; set; } = new List<ChatHistorial>();
    }
}
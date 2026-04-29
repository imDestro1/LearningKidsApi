using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningKidsAPI.Models
{
    [Table("DocenteAlumno")]
    public class DocenteAlumno
    {
        [Key]
        public int id { get; set; }

        public int? idDocente { get; set; }

        public int? idAlumno { get; set; }

        [ForeignKey("idDocente")]
        public Usuario? Docente { get; set; }

        [ForeignKey("idAlumno")]
        public Alumno? Alumno { get; set; }
    }
}
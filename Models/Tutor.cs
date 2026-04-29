using System.ComponentModel.DataAnnotations;

namespace LearningKidsAPI.Models
{
    public class Tutor
    {
        [Key]
        public int idTutor { get; set; }

        public string? nombre { get; set; }

        public string? correo { get; set; }

        public ICollection<Alumno> Alumnos { get; set; } = new List<Alumno>();
    }
}
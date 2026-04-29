using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningKidsAPI.Models
{
    public class Prueba
    {
        [Key]
        public int idPrueba { get; set; }

        public string? titulo { get; set; }

        public int? idTema { get; set; }

        public int? creadoPor { get; set; }

        [ForeignKey("idTema")]
        public Tema? Tema { get; set; }

        [ForeignKey("creadoPor")]
        public Usuario? Usuario { get; set; }

        public ICollection<Pregunta> Preguntas { get; set; } = new List<Pregunta>();
    }
}
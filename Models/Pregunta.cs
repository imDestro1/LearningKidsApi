using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningKidsAPI.Models
{
    public class Pregunta
    {
        [Key]
        public int idPregunta { get; set; }

        [Required]
        public string? texto { get; set; }

        public int? idPrueba { get; set; }

        [ForeignKey("idPrueba")]
        public Prueba? Prueba { get; set; }

        public ICollection<Respuesta> Respuestas { get; set; } = new List<Respuesta>();
    }
}
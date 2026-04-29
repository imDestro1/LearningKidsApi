using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningKidsAPI.Models
{
    public class Respuesta
    {
        [Key]
        public int idRespuesta { get; set; }

        [Required]
        public string? texto { get; set; }

        public bool? esCorrecta { get; set; }

        public int? idPregunta { get; set; }

        [ForeignKey("idPregunta")]
        public Pregunta? Pregunta { get; set; }
    }
}
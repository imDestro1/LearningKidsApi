using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningKidsAPI.Models
{
    public class Proyecto
    {
        [Key]
        public int idProyecto { get; set; }

        public string? nombre { get; set; }

        public string? descripcion { get; set; }

        public int? grado { get; set; }

        public int? idCampo { get; set; }

        public int? creadoPor { get; set; }

        [ForeignKey("idCampo")]
        public CampoFormativo? CampoFormativo { get; set; }

        [ForeignKey("creadoPor")]
        public Usuario? Usuario { get; set; }

        public ICollection<Tema> Temas { get; set; } = new List<Tema>();
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningKidsAPI.Models
{
    public class Tema
    {
        [Key]
        public int idTema { get; set; }

        public string? nombre { get; set; }

        public string? descripcion { get; set; }

        public int? idProyecto { get; set; }

        [ForeignKey("idProyecto")]
        public Proyecto? Proyecto { get; set; }

        public ICollection<Prueba> Pruebas { get; set; } = new List<Prueba>();
    }
}
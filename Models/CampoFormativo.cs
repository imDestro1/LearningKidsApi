using System.ComponentModel.DataAnnotations;

namespace LearningKidsAPI.Models
{
    public class CampoFormativo
    {
        [Key]
        public int idCampo { get; set; }

        public string? nombre { get; set; }

        public ICollection<Proyecto> Proyectos { get; set; } = new List<Proyecto>();
    }
}
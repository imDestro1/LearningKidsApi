using System.ComponentModel.DataAnnotations;

namespace LearningKidsAPI.Models
{
    public class Rol
    {
        [Key]
        public int idRol { get; set; }

        public string? nombre { get; set; }

        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningKidsAPI.Models
{
    public class Usuario
    {
        [Key]
        public int idUsuario { get; set; }

        public string? nombre { get; set; }

        public string? username { get; set; }

        public string? password { get; set; }

        public int? idRol { get; set; }

        [ForeignKey("idRol")]
        public Rol? Rol { get; set; }
    }
}

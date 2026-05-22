namespace LearningKidsAPI.DTOs
{
    public class DocenteAlumnosDetalleDTO
    {
        public int idDocente { get; set; }
        public string? nombreDocente { get; set; }
        public string? usernameDocente { get; set; }
        public List<AlumnoResumenDTO> alumnos { get; set; } = new();
    }

    public class AlumnoResumenDTO
    {
        public int idAlumno { get; set; }
        public string? nombreAlumno { get; set; }
        public string? usernameAlumno { get; set; }
        public int? grado { get; set; }
    }
}

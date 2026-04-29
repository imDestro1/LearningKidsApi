namespace LearningKidsAPI.DTOs
{
    public class DocenteAlumnoDTO
    {
        public int id { get; set; }
        public int? idDocente { get; set; }
        public int? idAlumno { get; set; }
        public string? nombreDocente { get; set; }
        public string? nombreAlumno { get; set; }
    }
}
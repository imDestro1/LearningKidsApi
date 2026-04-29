namespace LearningKidsAPI.DTOs
{
    public class PruebaCreateDTO
    {
        public string? titulo { get; set; }
        public int? idTema { get; set; }
        public List<PreguntaCreateDTO> preguntas { get; set; } = new List<PreguntaCreateDTO>();
    }
}
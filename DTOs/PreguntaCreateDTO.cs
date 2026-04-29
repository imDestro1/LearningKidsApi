namespace LearningKidsAPI.DTOs
{
    public class PreguntaCreateDTO
    {
        public string? texto { get; set; }
        public List<RespuestaCreateDTO> respuestas { get; set; } = new List<RespuestaCreateDTO>();
    }
}
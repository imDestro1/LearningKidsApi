namespace LearningKidsAPI.DTOs
{
    public class SubmitPruebaRequest
    {
        public int idAlumno { get; set; }
        public int idPrueba { get; set; }
        public List<int> respuestasSeleccionadas { get; set; } = new List<int>();
    }
}

namespace LearningKidsAPI.Models;

/// <summary>
/// Configuración especializada para el tutor de matemáticas
/// </summary>
public class MathTutorOptions
{
    public const string SectionName = "MathTutor";

    /// <summary>
    /// Grado de primaria (1-3)
    /// </summary>
    public int PrimaryGrade { get; set; } = 1;

    /// <summary>
    /// Temperatura más baja para respuestas consistentes y educativas
    /// </summary>
    public double Temperature { get; set; } = 0.3;

    /// <summary>
    /// TopP reducido para respuestas más determinísticas
    /// </summary>
    public double TopP { get; set; } = 0.8;

    /// <summary>
    /// Tokens máximos para explicaciones pedagógicas
    /// </summary>
    public int MaxOutputTokens { get; set; } = 1024;

    /// <summary>
    /// Modelo OpenAI
    /// </summary>
    public string Model { get; set; } = "gpt-4o";

    /// <summary>
    /// Usar respuestas con pasos desglosados
    /// </summary>
    public bool ShowSteps { get; set; } = true;

    /// <summary>
    /// Verificar y validar respuestas del estudiante
    /// </summary>
    public bool VerifyAnswers { get; set; } = true;
}

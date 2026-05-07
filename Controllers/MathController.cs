using LearningKidsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearningKidsAPI.Controllers;

/// <summary>
/// Controller para tutor de matemáticas de primaria baja (grados 1-3)
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MathController : ControllerBase
{
    private readonly MathTutorService _mathTutorService;
    private readonly ILogger<MathController> _logger;

    public MathController(MathTutorService mathTutorService, ILogger<MathController> logger)
    {
        _mathTutorService = mathTutorService;
        _logger = logger;
    }

    /// <summary>
    /// Chat con el tutor de matemáticas para resolver problemas, explicar o verificar respuestas
    /// </summary>
    /// <param name="request">Query del estudiante y respuesta opcional para verificar</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Respuesta educativa del tutor</returns>
    [HttpPost("chat")]
    public async Task<ActionResult<MathChatResponseDto>> Chat(
        [FromBody] MathChatRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request?.Query))
            {
                return BadRequest(new ErrorResponseDto { Message = "Query cannot be empty" });
            }

            var response = await _mathTutorService.ProcessMathQuery(
                request.Query,
                request.StudentAnswer,
                cancellationToken);

            return Ok(new MathChatResponseDto
            {
                Text = response.Text,
                Grade = response.Grade,
                HasLatex = response.HasLatex,
                IsAnswerVerification = response.IsAnswerVerification
            });
        }
        catch (HttpRequestException ex) when (ex.StatusCode.HasValue)
        {
            _logger.LogError(ex, "OpenAI request failed in Math Chat endpoint");
            return StatusCode((int)ex.StatusCode.Value,
                new ErrorResponseDto { Message = "OpenAI request failed. Check API key, quota, or billing." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Math Chat endpoint");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ErrorResponseDto { Message = "An error occurred while processing your request" });
        }
    }

    /// <summary>
    /// Genera un ejercicio matemático aleatorio para practicar
    /// </summary>
    /// <param name="topic">Tema opcional (sumas, restas, etc.)</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Un ejercicio matemático con su tema</returns>
    [HttpGet("exercise")]
    public async Task<ActionResult<MathExerciseResponseDto>> GenerateExercise(
        [FromQuery] string? topic = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var exercise = await _mathTutorService.GenerateExercise(topic, cancellationToken);

            return Ok(new MathExerciseResponseDto
            {
                Exercise = exercise.Exercise,
                Topic = exercise.Topic,
                Grade = exercise.Grade,
                HasLatex = exercise.HasLatex
            });
        }
        catch (HttpRequestException ex) when (ex.StatusCode.HasValue)
        {
            _logger.LogError(ex, "OpenAI request failed while generating exercise");
            return StatusCode((int)ex.StatusCode.Value,
                new ErrorResponseDto { Message = "OpenAI request failed. Check API key, quota, or billing." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating exercise");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ErrorResponseDto { Message = "Failed to generate exercise" });
        }
    }

    /// <summary>
    /// Verifica la respuesta del estudiante a un problema
    /// </summary>
    /// <param name="request">Problema y respuesta del estudiante</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Evaluación y retroalimentación del tutor</returns>
    [HttpPost("verify-answer")]
    public async Task<ActionResult<MathChatResponseDto>> VerifyAnswer(
        [FromBody] MathVerifyRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request?.Problem) || string.IsNullOrWhiteSpace(request?.Answer))
            {
                return BadRequest(new ErrorResponseDto { Message = "Problem and answer cannot be empty" });
            }

            var response = await _mathTutorService.ProcessMathQuery(
                request.Problem,
                request.Answer,
                cancellationToken);

            return Ok(new MathChatResponseDto
            {
                Text = response.Text,
                Grade = response.Grade,
                HasLatex = response.HasLatex,
                IsAnswerVerification = true
            });
        }
        catch (HttpRequestException ex) when (ex.StatusCode.HasValue)
        {
            _logger.LogError(ex, "OpenAI request failed while verifying answer");
            return StatusCode((int)ex.StatusCode.Value,
                new ErrorResponseDto { Message = "OpenAI request failed. Check API key, quota, or billing." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying answer");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ErrorResponseDto { Message = "An error occurred while verifying the answer" });
        }
    }
}

/// <summary>
/// Request para chat con el tutor
/// </summary>
public class MathChatRequestDto
{
    /// <summary>
    /// Pregunta o consulta del estudiante
    /// </summary>
    public string Query { get; set; } = string.Empty;

    /// <summary>
    /// Respuesta del estudiante (opcional, para verificación)
    /// </summary>
    public string? StudentAnswer { get; set; }
}

/// <summary>
/// Request para verificar respuesta
/// </summary>
public class MathVerifyRequestDto
{
    /// <summary>
    /// El problema/pregunta
    /// </summary>
    public string Problem { get; set; } = string.Empty;

    /// <summary>
    /// La respuesta del estudiante para verificar
    /// </summary>
    public string Answer { get; set; } = string.Empty;
}

/// <summary>
/// Response del chat con tutor de matemáticas
/// </summary>
public class MathChatResponseDto
{
    /// <summary>
    /// Respuesta educativa del tutor (puede incluir LaTeX)
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Grado de primaria
    /// </summary>
    public int Grade { get; set; }

    /// <summary>
    /// Si contiene notación LaTeX ($$ ... $$)
    /// </summary>
    public bool HasLatex { get; set; }

    /// <summary>
    /// Si es una verificación de respuesta
    /// </summary>
    public bool IsAnswerVerification { get; set; }
}

/// <summary>
/// Response con ejercicio matemático generado
/// </summary>
public class MathExerciseResponseDto
{
    /// <summary>
    /// Texto del ejercicio
    /// </summary>
    public string Exercise { get; set; } = string.Empty;

    /// <summary>
    /// Tema del ejercicio
    /// </summary>
    public string Topic { get; set; } = string.Empty;

    /// <summary>
    /// Grado de primaria para el cual es
    /// </summary>
    public int Grade { get; set; }

    /// <summary>
    /// Si contiene notación LaTeX
    /// </summary>
    public bool HasLatex { get; set; }
}

/// <summary>
/// Response de error genérico
/// </summary>
public class ErrorResponseDto
{
    /// <summary>
    /// Mensaje de error
    /// </summary>
    public string Message { get; set; } = string.Empty;
}

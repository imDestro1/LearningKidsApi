using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using LearningKidsAPI.Models;
using Microsoft.Extensions.Options;

namespace LearningKidsAPI.Services;

/// <summary>
/// Servicio especializado de tutor de matemáticas para primaria baja
/// Actúa como docente educativo para grados 1-3
/// </summary>
public class MathTutorService
{
    private readonly HttpClient _httpClient;
    private readonly MathTutorOptions _options;
    private readonly ILogger<MathTutorService> _logger;
    private readonly string _apiKey;

    private const string OpenAiApiUrl = "https://api.openai.com/v1/responses";

    /// <summary>
    /// System prompt que define el comportamiento del tutor
    /// </summary>
    private string SystemPrompt => $@"Eres un docente amable y paciente de matemáticas para niños de primaria (grado {_options.PrimaryGrade}).

Tu objetivo es:
1. Ayudar a los niños a entender conceptos matemáticos básicos
2. Resolver problemas paso a paso de manera clara y accesible
3. Explicar conceptos usando ejemplos cotidianos y divertidos
4. Generar ejercicios apropiados para el grado
5. Verificar las respuestas de forma constructiva y motivadora

Instrucciones importantes:
- Usa lenguaje simple y amigable, apropiado para niños
- Explica SIEMPRE los pasos de manera detallada
- Usa ejemplos con cosas que los niños conocen (dulces, juguetes, mascotas, etc.)
- Celebra los logros del niño, incluso si comete errores
- Si hay un error, explica por qué sin criticar
- Usa formato Markdown para ecuaciones con $$ ... $$ cuando sea necesario
- Para fracciones, decimales y símbolos, usa LaTeX
- Mantén las respuestas cortas pero claras
- Haz preguntas para verificar la comprensión
- Si el niño da una respuesta, verifica y da retroalimentación constructiva

Temas para grado {_options.PrimaryGrade}:
- Números del 0 al 100 (o mayor según el grado)
- Sumas y restas simples
- Concepto de cantidad y cardinalidad
- Reconocimiento de monedas y dinero básico
- Medidas simples (altura, distancia)
- Resolución de problemas simples
- Pensamiento lógico y patrones";

    public MathTutorService(
        HttpClient httpClient,
        IOptions<MathTutorOptions> options,
        IConfiguration configuration,
        ILogger<MathTutorService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;

        // Leer API key desde variable de entorno: OpenAI__ApiKey
        var apiKey = configuration["OpenAI:ApiKey"];
        
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException(
                "OpenAI API Key is not configured. Set environment variable: OpenAI__ApiKey");
        }

        _apiKey = apiKey;
    }

    /// <summary>
    /// Procesa una consulta del estudiante (resolver, explicar, generar ejercicio)
    /// </summary>
    public async Task<MathResponse> ProcessMathQuery(
        string query,
        string? studentAnswer = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var userMessage = BuildUserMessage(query, studentAnswer);

            var messages = new List<OpenAiInputMessage>
            {
                new() { Role = "system", Content = SystemPrompt },
                new() { Role = "user", Content = userMessage }
            };

            var text = await CallOpenAiAsync(messages, cancellationToken);

            return new MathResponse
            {
                Text = text,
                Grade = _options.PrimaryGrade,
                HasLatex = text.Contains("$$") || text.Contains("$"),
                IsAnswerVerification = !string.IsNullOrEmpty(studentAnswer)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing math query");
            throw;
        }
    }

    /// <summary>
    /// Genera un ejercicio matemático apropiado para el grado
    /// </summary>
    public async Task<MathExercise> GenerateExercise(
        string? topic = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var prompt = topic != null
                ? $"Genera UN ejercicio de matemáticas sobre {topic} para un niño de grado {_options.PrimaryGrade}. Solo el ejercicio, sin la solución."
                : $"Genera UN ejercicio de matemáticas apropiado y divertido para un niño de grado {_options.PrimaryGrade}. Solo el ejercicio, sin la solución.";

            var messages = new List<OpenAiInputMessage>
            {
                new() { Role = "system", Content = SystemPrompt },
                new() { Role = "user", Content = prompt }
            };

            var exerciseText = await CallOpenAiAsync(messages, cancellationToken);

            return new MathExercise
            {
                Exercise = exerciseText,
                Topic = topic ?? "General",
                Grade = _options.PrimaryGrade,
                HasLatex = exerciseText.Contains("$$") || exerciseText.Contains("$")
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating exercise");
            throw;
        }
    }

    private string BuildUserMessage(string query, string? studentAnswer)
    {
        if (!string.IsNullOrEmpty(studentAnswer) && _options.VerifyAnswers)
        {
            return $@"El estudiante tiene esta pregunta: {query}

El estudiante respondió: {studentAnswer}

Por favor, verifica la respuesta. Si es correcta, felicítalo. Si es incorrecta, explica por qué y ayúdalo a encontrar la respuesta correcta. Usa pasos claros.";
        }

        return query;
    }

    private async Task<string> CallOpenAiAsync(
        List<OpenAiInputMessage> messages,
        CancellationToken cancellationToken)
    {
        var request = new OpenAiResponseRequest
        {
            Model = _options.Model,
            Input = messages.Select(message => new OpenAiResponseInputItem
            {
                Role = message.Role,
                Content =
                [
                    new OpenAiResponseContentItem
                    {
                        Text = message.Content
                    }
                ]
            }).ToList(),
            Temperature = _options.Temperature,
            TopP = _options.TopP,
            MaxOutputTokens = _options.MaxOutputTokens
        };

        var jsonContent = JsonSerializer.Serialize(request, new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, OpenAiApiUrl)
        {
            Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
        };

        httpRequest.Headers.Add("Authorization", $"Bearer {_apiKey}");

        var response = await _httpClient.SendAsync(httpRequest, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("OpenAI API Error: {StatusCode} - {Content}", response.StatusCode, responseContent);
            throw new HttpRequestException($"OpenAI API returned {response.StatusCode}: {responseContent}", null, response.StatusCode);
        }

        using var jsonDocument = JsonDocument.Parse(responseContent);
        var text = ExtractOutputText(jsonDocument.RootElement);

        if (string.IsNullOrWhiteSpace(text))
        {
            throw new InvalidOperationException("No response from OpenAI API");
        }

        return text;
    }

    private static string ExtractOutputText(JsonElement root)
    {
        if (root.TryGetProperty("output_text", out var outputTextElement) &&
            outputTextElement.ValueKind == JsonValueKind.String)
        {
            return outputTextElement.GetString() ?? string.Empty;
        }

        if (!root.TryGetProperty("output", out var outputElement) ||
            outputElement.ValueKind != JsonValueKind.Array)
        {
            return string.Empty;
        }

        var builder = new StringBuilder();

        foreach (var outputItem in outputElement.EnumerateArray())
        {
            if (!outputItem.TryGetProperty("content", out var contentElement) ||
                contentElement.ValueKind != JsonValueKind.Array)
            {
                continue;
            }

            foreach (var contentItem in contentElement.EnumerateArray())
            {
                if (!contentItem.TryGetProperty("text", out var textElement) ||
                    textElement.ValueKind != JsonValueKind.String)
                {
                    continue;
                }

                builder.Append(textElement.GetString());
            }
        }

        return builder.ToString();
    }

    private sealed class OpenAiInputMessage
    {
        public string Role { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
    }

    private sealed class OpenAiResponseRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = string.Empty;

        [JsonPropertyName("input")]
        public List<OpenAiResponseInputItem> Input { get; set; } = [];

        [JsonPropertyName("temperature")]
        public double Temperature { get; set; }

        [JsonPropertyName("top_p")]
        public double TopP { get; set; }

        [JsonPropertyName("max_output_tokens")]
        public int MaxOutputTokens { get; set; }
    }

    private sealed class OpenAiResponseInputItem
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;

        [JsonPropertyName("content")]
        public List<OpenAiResponseContentItem> Content { get; set; } = [];
    }

    private sealed class OpenAiResponseContentItem
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "input_text";

        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }
}

/// <summary>
/// Respuesta del tutor de matemáticas
/// </summary>
public class MathResponse
{
    /// <summary>
    /// Respuesta del tutor (puede incluir LaTeX)
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Grado de primaria
    /// </summary>
    public int Grade { get; set; }

    /// <summary>
    /// Si contiene notación LaTeX
    /// </summary>
    public bool HasLatex { get; set; }

    /// <summary>
    /// Si es una verificación de respuesta del estudiante
    /// </summary>
    public bool IsAnswerVerification { get; set; }
}

/// <summary>
/// Ejercicio matemático generado
/// </summary>
public class MathExercise
{
    /// <summary>
    /// Texto del ejercicio
    /// </summary>
    public string Exercise { get; set; } = string.Empty;

    /// <summary>
    /// Tema del ejercicio
    /// </summary>
    public string Topic { get; set; } = "General";

    /// <summary>
    /// Grado de primaria
    /// </summary>
    public int Grade { get; set; }

    /// <summary>
    /// Si contiene notación LaTeX
    /// </summary>
    public bool HasLatex { get; set; }
}

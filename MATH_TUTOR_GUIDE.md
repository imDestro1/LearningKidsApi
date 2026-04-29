# 🧮 Tutor de Matemáticas - Primaria Baja (Grados 1-3)

## 📋 Descripción

El tutor de matemáticas es un endpoint especializado que actúa como **docente educativo de primaria**. Está optimizado para enseñar matemáticas a niños de grados 1-3 (edades 6-8 años).

### Características principales

✅ **Resolver problemas** - Paso a paso y con ejemplos cotidianos
✅ **Explicar conceptos** - Sumas, restas, dinero, medidas, patrones
✅ **Generar ejercicios** - Aleatorios o por tema específico
✅ **Verificar respuestas** - Retroalimentación constructiva y motivadora
✅ **Notación LaTeX** - Para ecuaciones y símbolos matemáticos
✅ **Contexto real** - Ejemplos con dulces, juguetes, mascotas

---

## 🎯 Endpoints

### 1. Chat con el Tutor: `/api/math/chat` (POST)

Resuelve problemas, explica conceptos, o verifica una respuesta del estudiante.

**Request:**
```json
{
  "query": "¿Qué es una suma? Dame un ejemplo.",
  "studentAnswer": null
}
```

**Parámetros:**
- `query` (string, requerido): La pregunta o consulta del estudiante
- `studentAnswer` (string, opcional): Respuesta del estudiante para verificar

**Response:**
```json
{
  "text": "Una suma es cuando juntamos cantidades. Por ejemplo, si tienes 2 caramelos y tu amigo te da 3 más, ahora tienes 5 caramelos. 2 + 3 = 5",
  "grade": 1,
  "hasLatex": false,
  "isAnswerVerification": false
}
```

---

### 2. Verificar Respuesta: `/api/math/verify-answer` (POST)

Verifica la respuesta del estudiante y proporciona retroalimentación detallada.

**Request:**
```json
{
  "problem": "Si tengo 7 manzanas y como 2, ¿cuántas me quedan?",
  "answer": "5"
}
```

**Response (respuesta correcta):**
```json
{
  "text": "¡Excelente! ¡Acertaste! 🎉 Tienes 5 manzanas. Mira cómo lo hicimos: Empezaste con 7 manzanas, comiste 2, así que 7 - 2 = 5. ¡Muy bien!",
  "grade": 1,
  "hasLatex": false,
  "isAnswerVerification": true
}
```

**Response (respuesta incorrecta):**
```json
{
  "text": "No, no es correcto. Pero no te preocupes, vamos a trabajar juntos. Empezaste con 7 manzanas. Si comes 2, necesitas restar. 7 - 2 = 5. Entonces te quedan 5 manzanas, no 3. ¡Inténtalo de nuevo!",
  "grade": 1,
  "hasLatex": false,
  "isAnswerVerification": true
}
```

---

### 3. Generar Ejercicio: `/api/math/exercise` (GET)

Genera un ejercicio matemático aleatorio o de un tema específico.

**Request sin parámetros:**
```
GET /api/math/exercise
```

**Request con tema:**
```
GET /api/math/exercise?topic=sumas
GET /api/math/exercise?topic=restas
```

**Response:**
```json
{
  "exercise": "Juan tiene 4 juguetes. Su hermana le da 3 más. ¿Cuántos juguetes tiene Juan ahora?",
  "topic": "sumas",
  "grade": 1,
  "hasLatex": false
}
```

---

## 💡 Ejemplos de Uso

### Ejemplo 1: Explicar un concepto

```bash
curl -X POST http://localhost:5125/api/math/chat \
  -H "Content-Type: application/json" \
  -d '{
    "query": "¿Qué es la resta? Dame un ejemplo fácil."
  }'
```

**Respuesta esperada:** Explicación simple con ejemplos cotidianos

---

### Ejemplo 2: Resolver un problema complejo

```bash
curl -X POST http://localhost:5125/api/math/chat \
  -H "Content-Type: application/json" \
  -d '{
    "query": "Laura compró 3 lápices a la tienda. Su abuela le regala 5 lápices más. ¿Cuántos lápices tiene Laura en total?"
  }'
```

**Respuesta esperada:** Solución paso a paso con contexto real

---

### Ejemplo 3: Verificar una respuesta

```bash
curl -X POST http://localhost:5125/api/math/verify-answer \
  -H "Content-Type: application/json" \
  -d '{
    "problem": "6 + 4 = ?",
    "answer": "10"
  }'
```

**Respuesta esperada:** Felicitación y refuerzo positivo

---

### Ejemplo 4: Generar un ejercicio de sumas

```bash
curl -X GET http://localhost:5125/api/math/exercise?topic=sumas
```

**Respuesta esperada:** Un ejercicio nuevo de sumas

---

## ⚙️ Configuración

### appsettings.json

```json
{
  "MathTutor": {
    "PrimaryGrade": 1,              // Grado de primaria (1-3)
    "Temperature": 0.3,              // Baja temperatura para respuestas consistentes
    "TopP": 0.8,                     // Nucleus sampling reducido
    "MaxOutputTokens": 1024,         // Máximo de tokens
    "Model": "gpt-4o",               // Modelo OpenAI
    "ShowSteps": true,               // Mostrar pasos en soluciones
    "VerifyAnswers": true            // Habilitar verificación de respuestas
  }
}
```

### Cambiar grado de primaria

Para usar con grado 2 o 3, actualiza `PrimaryGrade`:

```json
"MathTutor": {
  "PrimaryGrade": 2,  // Para 2do grado
  ...
}
```

El system prompt se ajustará automáticamente según el grado.

---

## 🔒 Seguridad

- ✅ API Key desde variable de entorno (`OpenAI__ApiKey`)
- ✅ No expuesta al cliente (Desk/Móvil)
- ✅ Backend hace todas las llamadas a OpenAI
- ✅ Respuestas seguras y apropiadas para niños

---

## 📊 Características Especiales

### LaTeX para notación matemática

Cuando el tutor retorna `hasLatex: true`, la respuesta incluye LaTeX:

```
Para fracciones: $\frac{1}{2}$
Para ecuaciones: $$x + 5 = 10$$
```

El cliente debe renderizar usando una librería como KaTeX o MathJax.

### Verificación constructiva

Las respuestas incorrectas nunca son negativas:

❌ **Evitar:** "Está mal, deberías haberlo sabido"

✅ **Hacer:** "No es correcto. Vamos a trabajar juntos. [Explicación clara]"

---

## 5️⃣ Próxima Fase: Streaming SSE

Para convertir a **streaming SSE** (Server-Sent Events), se puede:

- Respuesta en tiempo real (palabra por palabra)
- Menor latencia percibida
- Mejor experiencia para usuarios
- API Key sigue segura en backend

---

## 🔑 API Key de OpenAI

**IMPORTANTE:** La API key **NUNCA** va en el código.

### Configurar la API Key

1. Obtén tu key en https://platform.openai.com/api/keys
2. Configura como variable de entorno: `OpenAI__ApiKey`
3. Lee las instrucciones detalladas en: [OPENAI_API_KEY_SETUP.md](OPENAI_API_KEY_SETUP.md)

### Verificar que Funciona

```powershell
$env:OpenAI__ApiKey
# Debe mostrar: sk-...
```

---

## 📝 Notas

- Temperatura baja (0.3) asegura respuestas educativas y consistentes
- Parámetros de OpenAI están optimizados para primaria baja
- El system prompt es automático y pedagógico
- Cada respuesta es personalizada al grado actual

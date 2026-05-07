# 🔑 Configuración de API Key OpenAI

## ⚠️ IMPORTANTE: Variables de Entorno

La API key de OpenAI **NUNCA** debe estar en el código. Se configura como **variable de entorno**.

---

## 1️⃣ Obtener la API Key

1. Ve a https://platform.openai.com/api/keys
2. Inicia sesión con tu cuenta OpenAI
3. Haz click en "Create new secret key"
4. Copia la key (empieza con `sk-...`)
5. **Guárdala en un lugar seguro** (solo aparece una vez)

---

## 2️⃣ Configurar Variable de Entorno

### 🪟 En Windows (Permanente)

**Opción A: PowerShell (como Administrador)**
```powershell
[Environment]::SetEnvironmentVariable("OpenAI__ApiKey", "sk-tu-api-key-aqui", "User")
```

**Opción B: CMD (como Administrador)**
```cmd
setx OpenAI__ApiKey "sk-tu-api-key-aqui"
```

Luego **cierra y abre una nueva ventana de terminal** para que se aplique.

### 🐧 En Linux/macOS (Temporal)
```bash
export OpenAI__ApiKey="sk-tu-api-key-aqui"
```

### 🐧 En Linux/macOS (Permanente - agregar a ~/.bashrc o ~/.zshrc)
```bash
echo 'export OpenAI__ApiKey="sk-tu-api-key-aqui"' >> ~/.bashrc
source ~/.bashrc
```

---

## 3️⃣ Verificar que Funciona

### Opción A: Verificar en PowerShell
```powershell
$env:OpenAI__ApiKey
```

Debe mostrar: `sk-...` (tu API key)

### Opción B: Dentro del código (debugging)
En `MathTutorService.cs`, el constructor mostrará un error si no está configurada:

```
OpenAI API Key is not configured. Set environment variable: OpenAI__ApiKey
```

---

## 4️⃣ Configuración para Desarrollo en VS Code

Si quieres variables locales solo para desarrollo sin que quede en Git:

### Crear archivo `.env.local` (NO commitear a Git)
```
OpenAI__ApiKey=sk-tu-api-key-aqui
```

### Agregar a `.gitignore`
```
.env.local
.env
```

---

## 5️⃣ Testing en LearningKidsAPI.http

Una vez configurada la variable de entorno, puedes probar:

1. Abre `LearningKidsAPI.http`
2. Ve a la sección "🧮 TUTOR DE MATEMÁTICAS"
3. Haz click en "Send Request" en cualquier endpoint
4. Debe funcionar sin errores

---

## 6️⃣ Estructura de la Configuración

La API key se lee de esta forma:

```
appsettings.json
    ↓
appsettings.Development.json
    ↓
appsettings.Production.json
    ↓
Variables de Entorno ← **AQUÍ se obtiene OpenAI__ApiKey**
    ↓
IConfiguration
    ↓
MathTutorService
```

---

## 🔒 Seguridad

✅ **Correcto:**
- API key en variable de entorno
- NO en appsettings.json
- NO en código
- NO en Git

❌ **NUNCA hacer:**
```json
{
  "OpenAI": {
    "ApiKey": "sk-..."  // ❌ ¡NUNCA!
  }
}
```

---

## 📝 Troubleshooting

### Error: "OpenAI API Key is not configured"

**Causas posibles:**
1. ❌ Variable de entorno no configurada
2. ❌ Terminal/IDE abierto ANTES de configurar (cerrar y abrir de nuevo)
3. ❌ Nombre de variable incorrecto (debe ser `OpenAI__ApiKey` exactamente)

**Solución:**
```powershell
# En PowerShell, verificar:
$env:OpenAI__ApiKey

# Si está vacío, configurar:
[Environment]::SetEnvironmentVariable("OpenAI__ApiKey", "sk-...", "User")

# Cerrar VS Code completamente y abrir de nuevo
```

### Error: "401 Unauthorized" de OpenAI API

**Causa:** API key incorrecta o expirada

**Solución:**
1. Ve a https://platform.openai.com/api/keys
2. Verifica que la key esté activa
3. Si expiró, crea una nueva
4. Actualiza la variable de entorno

---

## 🚀 Próximo Paso

Una vez configurada la API key, puedes:
- ✅ Probar los endpoints en `LearningKidsAPI.http`
- ✅ Hacer requests desde Postman
- ✅ Integrar en tu frontend (Desk/Móvil)

¡Listo! 🎉

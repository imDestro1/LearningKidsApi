# ✅ Checklist de Configuración - Tutor de Matemáticas

## 🔧 Configuración Inicial (Una sola vez)

- [ ] **Obtener API Key OpenAI**
  - [ ] Ve a https://platform.openai.com/api/keys
  - [ ] Crea una nueva secret key
  - [ ] Copia la key (empieza con `sk-...`)

- [ ] **Configurar Variable de Entorno**
  - [ ] Windows: Ejecuta en PowerShell como Administrador:
    ```powershell
    [Environment]::SetEnvironmentVariable("OpenAI__ApiKey", "sk-tu-api-key", "User")
    ```
  - [ ] Linux/Mac: Agrega a `~/.bashrc` o `~/.zshrc`:
    ```bash
    export OpenAI__ApiKey="sk-tu-api-key"
    ```
  - [ ] Cierra VS Code completamente
  - [ ] Abre una nueva terminal/VS Code

- [ ] **Verificar Configuración**
  ```powershell
  $env:OpenAI__ApiKey
  # Debe mostrar: sk-...
  ```

---

## 🚀 Primer Test (Una sola vez)

- [ ] Abre `LearningKidsAPI.http`
- [ ] Ve a sección "🧮 TUTOR DE MATEMÁTICAS"
- [ ] Haz click en "Send Request" del primer ejemplo
- [ ] Debe ver respuesta del tutor (sin error de API key)

---

## 📁 Archivos Generados

| Archivo | Propósito |
|---------|-----------|
| `Models/MathTutorOptions.cs` | Configuración pedagógica |
| `Services/MathTutorService.cs` | Lógica del tutor |
| `Controllers/MathController.cs` | 3 endpoints (/chat, /exercise, /verify-answer) |
| `MATH_TUTOR_GUIDE.md` | Documentación de uso |
| `OPENAI_API_KEY_SETUP.md` | Guía de API key |
| `.env.example` | Template de variables |
| `.gitignore` | Para no commitar secrets |

---

## 📊 Endpoints Disponibles

```
POST   /api/math/chat              # Chat con tutor
GET    /api/math/exercise          # Generar ejercicio
POST   /api/math/verify-answer     # Verificar respuesta
```

---

## 🔒 Seguridad Verificada

✅ API Key en variable de entorno (NUNCA en código)
✅ Backend hace llamadas a OpenAI (NO cliente)
✅ .gitignore configurable
✅ .env.local no será commiteado

---

## 🆘 Si Algo Falla

### Error: "OpenAI API Key is not configured"
→ Ve a **OPENAI_API_KEY_SETUP.md** sección "Troubleshooting"

### Error: "401 Unauthorized" 
→ La API key es incorrecta o expirada (obtener nueva)

### Error: MathTutorService no se inyecta
→ Verificar que está registrado en `Program.cs`
→ Verificar que `IConfiguration` se pasa en constructor

---

## 📝 Notas Importantes

- Una sola variable de entorno: `OpenAI__ApiKey`
- Formato: `sk-...` (mínimo 20 caracteres)
- Se lee automáticamente en startup
- No requiere appsettings.json especial

---

**¿Listo para empezar?** → Ve a **OPENAI_API_KEY_SETUP.md** 🚀

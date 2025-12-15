# 🚀✨ Proyecto de Datos Masivos — ULACIT

**Proyecto de Datos Masivos en la ULACIT**

- **Integrantes:**
  - Jennifer Vega Chacón
  - Bernard de Jesus Paniagua Alfaro
  - Delvis Gonzalez Gonzalez
  - Yereth Soto Zuñiga

---

## 🌟 Introducción (divertida)

¡Bienvenid@s al proyecto de Datos Masivos! 🎉
Este repositorio contiene una pequeña aplicación de Azure Functions que procesa y agrega datos de un conjunto de taxis (tabla `YellowTrips2019`) para obtener estadísticas por `VendorID`. 📊🚕

Disfruta explorando los datos y aprendiendo cómo desplegar funciones serverless que consumen una base de datos SQL. 💡☁️

---

## 🧭 Descripción ligera — ¿Qué hace el proyecto?

- **Tipo de aplicación:** Azure Functions (worker aislado - `dotnet-isolated`).
- **Función principal:** `ProcesarDatos` — Trigger HTTP (GET).
- **Qué hace:** conecta a una base de datos SQL (cadena en `SqlConnection`), ejecuta una consulta que filtra y agrega registros de `YellowTrips2019` (filtra `trip_distance > 2`) y devuelve un JSON con el conteo por `VendorID`.
- **Respuesta:** JSON con objetos `{ VendorID, Count }` y código HTTP 200 (o 500 si hay error).

---

## ⚙️ Archivos clave detectados

- `Program.cs` — arranca la aplicación de Functions.
- `ProcesarDatos.cs` — implementación de la función HTTP y la consulta SQL.
- `local.settings.json` — configuración local (ej. `SqlConnection`).
- `host.json` — configuración del host (telemetría, concurrencia).

---

## 🛠️ Cómo ejecutar localmente (rápido)

Requisitos previos:
- .NET 8 SDK instalado
- Azure Functions Core Tools (`func`) instalado

Pasos:

1. Ajusta la cadena de conexión en `local.settings.json` (reemplaza `{your_password}` por la contraseña correcta):

2. Compila la solución:

```powershell
dotnet build
```

3. Ejecuta las funciones localmente (ejemplo usando la salida compilada):

```powershell
func start --script-root ./bin/Debug/net8.0
```

4. Llama al endpoint (GET):

```
http://localhost:7071/api/ProcesarDatos
```

Nota: la función está con `AuthorizationLevel.Function`; si tu host requiere una key, añade `?code=<FUNCTION_KEY>` a la URL o usa la clave que genere el host local.

---

## 🔐 Configuración importante

- Edita `local.settings.json` y completa `SqlConnection` con las credenciales correctas.
- Si usas emuladores (Azurite) o recursos locales, asegúrate de que `AzureWebJobsStorage` esté configurado apropiadamente.

---



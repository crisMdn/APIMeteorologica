# 🌦️ API Meteorológica en .NET

Este proyecto es una API REST construida en C# con ASP.NET Core, que actúa como intermediaria entre una aplicación cliente y un proveedor externo de datos meteorológicos (Visual Crossing). Está diseñada bajo principios de seguridad, escalabilidad y buenas prácticas de arquitectura.

---

## 🔍 Objetivo

Crear una API capaz de consultar el estado del clima de una ciudad específica, utilizando una API externa, protegiendo las claves sensibles y optimizando el rendimiento mediante técnicas modernas como la **inyección de dependencias** y (en futuras fases) **almacenamiento en caché con Redis**.

---

## 🧠 ¿Por qué crear una API intermedia?

En lugar de consumir directamente la API de Visual Crossing desde el frontend, se construyó esta capa intermedia por las siguientes razones:

- 🔐 **Seguridad**: protege la clave de API, evitando exponerla al cliente.
- 🧩 **Desacoplamiento**: si cambia el proveedor externo, no afecta directamente al cliente.
- 🔄 **Flexibilidad**: permite agregar validaciones, transformar datos o integrarse con múltiples servicios.
- 📈 **Escalabilidad**: permite manejar más tráfico, integrar autenticación, cache, etc.

---

## 🧩 Inyección de Dependencias

Se utilizó **inyección de dependencias** para conectar el controlador (`WeatherController`) con el servicio (`WeatherService`) sin acoplarlos directamente. Esto permite:

- Código más limpio y mantenible.
- Facilita pruebas unitarias.
- Modularización de la lógica.

```csharp
private readonly WeatherService _weatherService;

public WeatherController(WeatherService weatherService)
{
    _weatherService = weatherService;
}
```

---

## 📦 Estructura del Proyecto

```
/WeatherApi
│
├── Controllers
│   └── WeatherController.cs
│
├── Services
│   └── WeatherService.cs
│
├── appsettings.json     // Configuración de clave API y URL base
├── Program.cs           // Registro de servicios (inyección)
└── README.md
```

---

## 📡 Funcionamiento del Servicio

El servicio `WeatherService` hace una petición HTTP al endpoint de Visual Crossing y devuelve la respuesta como JSON al cliente. Se maneja la validación del estado de la respuesta y posibles errores.

```csharp
public async Task<string> GetWeatherAsync(string city)
{
    var url = $"{_baseUrl}/{city}?unitGroup=metric&key={_apiKey}&contentType=json";
    var response = await _httpClient.GetAsync(url);

    if (!response.IsSuccessStatusCode)
    {
        return $"Error: {response.StatusCode}";
    }

    return await response.Content.ReadAsStringAsync();
}
```

---

## 🔄 Plan Futuro: Integración con Redis

En la siguiente fase se integrará **Redis** para almacenar en caché las respuestas por ciudad durante cierto tiempo (por ejemplo, 12 horas). Esto permitirá:

- ⚡ Mejorar la velocidad de respuesta.
- 🔁 Evitar llamadas repetitivas innecesarias.
- 📉 Reducir la carga sobre la API externa.

La clave de cache será el nombre de la ciudad, y se usará un tiempo de expiración (`EX`).

---

## 🚀 Ejemplo de Consulta

```http
GET http://localhost:5130/weather/SanSalvador
```

---

## ✅ Tecnologías Usadas

- ASP.NET Core 7
- C#
- HttpClient
- Visual Crossing API
- Swagger (documentación automática)
- Redis (próximamente)

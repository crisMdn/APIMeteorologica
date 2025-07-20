# 🌦️ API Meteorológica en .NET

Este proyecto es una API REST construida en C# con ASP.NET Core, que actúa como intermediaria entre una aplicación cliente y un proveedor externo de datos meteorológicos (Visual Crossing). Está diseñada bajo principios de seguridad, escalabilidad y buenas prácticas de arquitectura.

---

## 🔍 Objetivo

Crear una API capaz de consultar el estado del clima de una ciudad específica, utilizando una API externa, protegiendo las claves sensibles y optimizando el rendimiento mediante técnicas modernas como la **inyección de dependencias** y **almacenamiento en caché con Redis**.

---

## 🧠 ¿Por qué crear una API intermedia?

En lugar de consumir directamente la API de Visual Crossing desde el frontend, se construyó esta capa intermedia por las siguientes razones:

- 🔐 **Seguridad**: protege la clave de API, evitando exponerla al cliente.
- 🧩 **Desacoplamiento**: si cambia el proveedor externo, no afecta directamente al cliente.
- 🔄 **Flexibilidad**: permite agregar validaciones, transformar datos o integrarse con múltiples servicios.
- 📈 **Escalabilidad**: permite manejar más tráfico, integrar autenticación, cache, etc.

---

## 🧩 Inyección de Dependencias

Se utilizó **inyección de dependencias** para conectar el controlador (`WeatherController`) con los servicios (`WeatherService` y `RedisCacheService`) sin acoplarlos directamente. Esto permite:

- Código más limpio y mantenible.
- Facilita pruebas unitarias.
- Modularización de la lógica.

```csharp
private readonly WeatherService _weatherService;
private readonly RedisCacheService _redisCacheService;

public WeatherController(WeatherService weatherService, RedisCacheService redisCacheService)
{
    _weatherService = weatherService;
    _redisCacheService = redisCacheService;
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
│   ├── WeatherService.cs
│   └── RedisCacheService.cs
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

## ⚙️ Integración con Redis (Caché)

### 🔧 ¿Qué se hizo?

1. Se creó el servicio `RedisCacheService` con dos métodos principales:
   - `SetAsync(key, value, expiration)` para guardar en caché.
   - `GetAsync(key)` para obtener el valor cacheado.

2. Se registró el cliente Redis en `Program.cs`:

```csharp
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
});
```

3. Se registró el servicio Redis para inyección:

```csharp
builder.Services.AddSingleton<RedisCacheService>();
```

4. En `WeatherController`, se añadió la lógica para primero consultar la caché antes de hacer la llamada HTTP externa:

```csharp
var cachedResult = await _redisCacheService.GetAsync(city);
if (!string.IsNullOrEmpty(cachedResult))
{
    return Content(cachedResult, "application/json");
}

var result = await _weatherService.GetWeatherAsync(city);
await _redisCacheService.SetAsync(city, result, TimeSpan.FromHours(1));
return Content(result, "application/json");
```

---

## ✅ Beneficios del uso de Redis

- ⚡ **Velocidad**: disminuye el tiempo de respuesta al cliente.
- 📉 **Menos carga** sobre la API externa.
- 🔁 **Datos persistentes temporalmente** que evitan consultas repetidas.
- 🚀 **Preparado para escalar** a proyectos más grandes.

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
- Redis (caché con StackExchange.Redis)

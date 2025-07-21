using ApiClima.Service; // 👈 Asegúrate que coincida con el namespace del archivo RedisCacheService.cs

// 🛠️ Crea el constructor de la app y carga configuración, servicios y entorno
var builder = WebApplication.CreateBuilder(args);

// 📦 Registra servicios (dependencias) que usará la aplicación
builder.Services.AddControllers();                 // Controladores de la API
builder.Services.AddEndpointsApiExplorer();       // Soporte para Swagger con minimal APIs
builder.Services.AddSwaggerGen();                 // Documentación Swagger

builder.Services.AddSingleton<WeatherService>();  // Inyección de dependencia para WeatherService
builder.Services.AddSingleton(new RedisCacheService("localhost:6379")); // Redis como singleton

// ⚙️ Construye la instancia final de la aplicación con toda la configuración anterior
var app = builder.Build(); // Ahora podés usar app para middlewares, rutas, y ejecutar la app

// 🔍 Habilita Swagger SOLO en entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 🔐 Middlewares para seguridad y permisos
app.UseHttpsRedirection(); // Redirige HTTP a HTTPS (comunicación cifrada)
app.UseAuthorization();    // Verifica permisos (luego de la autenticación)

// 🚦 Mapea rutas de los controladores con atributos [Route]
app.MapControllers();

// 🚀 Ejecuta la aplicación (arranca el servidor)
app.Run();


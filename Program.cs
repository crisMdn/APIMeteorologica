//registra WeatherService como dependencia 
using ApiClima.Service; // 👈 Asegurate que coincida con el namespace del archivo RedisCacheService.cs

var builder = WebApplication.CreateBuilder(args);

// Agrega servicios de controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Inyectar configuracion Automaticamente 
builder.Services.AddSingleton<WeatherService>();
//INTEGRACION DE REDIS 
builder.Services.AddSingleton(new RedisCacheService("localhost:6379"));


var app = builder.Build();

// Habilitar Swagger en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Mapea controladores (muy importante)
app.MapControllers();

app.Run();

/*
using Microsoft.AspNetCore.Mvc;
// 👉 Importa el espacio de nombres necesario para trabajar con controladores, rutas y respuestas HTTP (IActionResult).

[ApiController]
// 👉 Declara esta clase como un controlador de API REST.
// 👉 Habilita validaciones automáticas y generación de respuestas estándar.

[Route("[controller]")]
// 👉 Define la ruta base del controlador.
// 👉 [controller] se reemplaza automáticamente con el nombre de la clase sin el sufijo "Controller".
// 👉 Ejemplo: WeatherController se convierte en /weather.

public class WeatherController : ControllerBase
// 👉 Declara la clase WeatherController que hereda de ControllerBase.
// 👉 ControllerBase te da acceso a métodos como Ok(), BadRequest(), NotFound(), etc.
{
    [HttpGet("{city}")]
    // 👉 Este método responderá a solicitudes HTTP GET.
    // 👉 "{city}" indica que se espera un valor dinámico en la URL, por ejemplo: /weather/SanSalvador

    public IActionResult GetWeather(string city)
    // 👉 Método que recibe el parámetro "city" como string desde la URL.
    // 👉 Retorna un IActionResult (puede ser Ok, NotFound, BadRequest, etc.)
    {
        // 👉 Crea un objeto anónimo con datos simulados (hardcodeados)
        var result = new 
        {
            City = city,             // 👉 Muestra la ciudad consultada.
            Temperature = "25°C",    // 👉 Temperatura fija de ejemplo.
            Condition = "Sunny"      // 👉 Condición climática fija de ejemplo.
        };

        // 👉 Devuelve una respuesta HTTP 200 OK con el objeto como JSON.
        return Ok(result);
    }
}

// 🌐 Ejemplos para probar en el navegador o Postman:
// http://localhost:5130/weather/SanSalvador
// http://localhost:5130/weather/{ciudad}
*/

//DATOS GENERAL:  Inyecta Weather service y responde el cliente 
using Microsoft.AspNetCore.Mvc;
// 👉 Importa lo necesario para trabajar con controladores, rutas y respuestas HTTP (IActionResult)

[ApiController]
// 👉 Declara la clase como un controlador de tipo API REST

[Route("[controller]")]
// 👉 Define la ruta base: si la clase se llama WeatherController, la ruta será /weather

public class WeatherController : ControllerBase
{
    private readonly WeatherService _weatherService;
    // 👉 Dependencia inyectada del servicio que consulta la API externa

    public WeatherController(WeatherService weatherService)
    {
        _weatherService = weatherService;
        // 👉 Se inyecta mediante el constructor usando el sistema de dependencias de .NET
    }

    [HttpGet("{city}")]
    // 👉 Responde a solicitudes GET con una ciudad como parámetro. Ej: /weather/SanSalvador

    public async Task<IActionResult> GetWeather(string city)
    {
        var result = await _weatherService.GetWeatherAsync(city);
        // 👉 Llama al método del servicio para obtener el clima real desde Visual Crossing

        return Content(result, "application/json");
        // 👉 Devuelve la respuesta como JSON plano (ya viene en formato JSON desde la API externa)
    }
}

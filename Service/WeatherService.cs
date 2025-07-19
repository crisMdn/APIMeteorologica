// 🌦️ WeatherService.cs
// Esta clase se encarga de consumir la API externa de Visual Crossing para obtener información del clima.

using System.Net.Http;                 // 👉 Necesario para hacer solicitudes HTTP (GET, POST, etc.)
using System.Threading.Tasks;         // 👉 Permite trabajar con tareas asincrónicas (async/await)
using Microsoft.Extensions.Configuration; // 👉 Se usa para acceder a la configuración del archivo appsettings.json

public class WeatherService //declaracion de clase que usare para comunicarme con la API externa 
{    //variables privados y solo lectura readonly que se iniciliazaran en el constructor 
    private readonly HttpClient _httpClient; // 👉 Cliente HTTP para hacer peticiones a la API externa
    private readonly string _apiKey;         // 👉 Almacena la clave de API obtenida del archivo de configuración
    private readonly string _baseUrl;        // 👉 Almacena la URL base de la API externa
    
    //constructor 
    public WeatherService(IConfiguration configuration)
    {
        // 👉 Se crea una nueva instancia(objeto) del cliente HTTP
        _httpClient = new HttpClient();

        // 👉 Se accede a los valores definidos en appsettings.json bajo la sección WeatherSettings
        _apiKey = configuration["WeatherSettings:ApiKey"];       // Ej: "3UNPE4JJNCBJH3SJ2JB6N9KRK"
        _baseUrl = configuration["WeatherSettings:BaseUrl"];     // Ej: "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline"
    }

    public async Task<string> GetWeatherAsync(string city)
    {
        // 👉 Se arma la URL completa con parámetros: ciudad, unidad de medida, clave y tipo de contenido (JSON)
        var url = $"{_baseUrl}/{city}?unitGroup=metric&key={_apiKey}&contentType=json";

        // 👉 Se hace la solicitud GET a la API de Visual Crossing
        var response = await _httpClient.GetAsync(url);

        // 👉 Si la respuesta no es exitosa (por ejemplo, error 400 o 500), se devuelve el código de error
        // 👉 Verifica si la respuesta HTTP fue exitosa (verificando la varibale reponse).
        // 👉 Si NO lo fue (!), devuelve un mensaje de error con el código de estado (como 404, 500, etc.)
        // 👉 Esto evita procesar respuestas fallidas o vacías desde la API externa.
        if (!response.IsSuccessStatusCode) //esta es propiedad de la clase HttpReponse y devuelve True si esta entre 200 y 299
        {
            return $"Error :c : {response.StatusCode}";
        }

        // 👉 Si todo está bien, se lee y se devuelve el contenido de la respuesta como string (ya viene en JSON)
        return await response.Content.ReadAsStringAsync();
    }
}

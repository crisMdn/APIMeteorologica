//weatherService me sirve para consumir la API de Visual Crossing 

using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class WeatherService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _baseUrl;

    public WeatherService(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        _apiKey = configuration["WeatherSettings:ApiKey"];
        _baseUrl = configuration["WeatherSettings:BaseUrl"];
    }

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
}

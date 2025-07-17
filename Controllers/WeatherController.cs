using Microsoft.AspNetCore.Mvc; //indicador que sirve para usar controladores, apicontrollers, route, httpget, Iactionsresult (namespace)

[ApiController] //controlador de API 
[Route("[controller]")] //define la url, route = atributo para definir ulr que usara el controlador; el controller se remplaza por el nombre del controlador (sin escribir la ruta) 
//el controlller sera remplazado al momento de compilar con el nombre original del controllador. 
public class WeatherController : ControllerBase //definicion de la nueva clase indicando que se le pasaran las caracteristicas (herencia) (:) de ControllerBase (clase ya incluida en ASP)
//heredo las funcionalidades de Controllarbase
{ 
    [HttpGet("{city}")]  //atributo que indica que el metodo que viene a continuacion respondera a la solicitud httpget, (obtener datos), el parametro city indica que el get necesitara ese tipo de parametro para la consulta
    public IActionResult GetWeather(string city) //define un metodo en el controlador. //IActionResut = tipo de respuesta like : ok, not founde etc. -GetWeather espera el parametro city string 
    {
        // Respuesta de prueba (hardcodeada)
        var result = new //creacion de variable result and guardara un objeto anonimo
        {
            City = city, //datos del objeto (propiedades)
            Temperature = "25°C",
            Condition = "Sunny"
        };

        return Ok(result); //return devolvera respuesta al cliente, ok siendo el metodo generado con el http dado en IAactionsResult y devolviendo result donde se aloja el objeto 
    }
}



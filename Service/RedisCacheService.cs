using StackExchange.Redis; // 👉 Librería para conectarse a Redis
using System;
using System.Threading.Tasks;

namespace ApiClima.Service
{
    public class RedisCacheService
    {
        private readonly IDatabase _cache;      // 👉 Objeto que representa la base de datos de Redis
        private readonly ConnectionMultiplexer _redis; // 👉 Conexión activa con el servidor Redis

        // 👉 Constructor que recibe la cadena de conexión (ej: "localhost:6379")
        public RedisCacheService(string connectionString)
        {
            _redis = ConnectionMultiplexer.Connect(connectionString); // 🔌 Establece la conexión
            _cache = _redis.GetDatabase(); // 📂 Obtiene la base de datos por defecto de Redis
        }

        // 👉 Método para guardar datos en caché (clave, valor y tiempo de expiración)
        public async Task SetAsync(string key, string value, TimeSpan expiration)
        {
            await _cache.StringSetAsync(key, value, expiration); // 💾 Guarda en Redis con expiración
        }

        // 👉 Método para obtener datos de caché por clave
        public async Task<string> GetAsync(string key)
        {
            return await _cache.StringGetAsync(key); // 🔍 Devuelve el valor si existe o null
        }
    }
}

// Modifica la clase ConexionBD
using Microsoft.Extensions.Configuration;

namespace TodoApi.Conexion
{
    public class ConexionBD
    {
        private readonly IConfiguration _configuration;
        private string connectionString = string.Empty;

        public ConexionBD(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("conexion") ?? "Cadena de conexión predeterminada";
        }

        public string cadenaSQL()
        {
            return connectionString;
        }
    }
}

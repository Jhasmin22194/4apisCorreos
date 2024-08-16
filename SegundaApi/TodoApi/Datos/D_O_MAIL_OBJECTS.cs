using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using TodoApi.Conexion;
using TodoApi.Modelo;

namespace TodoApi.Datos
{
    public class D_O_MAIL_OBJECTS
    {
        private readonly IConfiguration _configuration;
        private readonly ConexionBD _cn;

        public D_O_MAIL_OBJECTS(IConfiguration configuration)
        {
            _configuration = configuration;
            _cn = new ConexionBD(_configuration);
        }

        public async Task<List<M_O_MAIL_OBJECTS>> MostrarDatos()
        {
            var lista = new List<M_O_MAIL_OBJECTS>();

            try
            {
                // Crear la conexión a la base de datos
                using (var sql = new SqlConnection(_cn.cadenaSQL()))
                {

                    // Construir la consulta SQL
                    string query = @"
                        SELECT M.MAILITM_FID, C.CUSTOMER_PHONE_NO, C.COUNTRY_CD,C.CUSTOMER_CITY, T.LOCAL_EVENT_TYPE_NM
                        FROM L_MAILITMS M 
                        JOIN L_MAILITM_CUSTOMERS C ON M.MAILITM_PID = C.MAILITM_PID
                        JOIN C_EVENT_TYPES E ON E.EVENT_TYPE_CD = M.EVT_TYPE_CD
                        JOIN CT_EVENT_TYPES T ON T.EVENT_TYPE_CD = E.EVENT_TYPE_CD
                        WHERE C.SENDER_PAYEE_IND ='A' 
                        AND T.EVENT_TYPE_CD = 32 
                        AND C.CUSTOMER_PHONE_NO IS NOT NULL ";
                    // Crear el comando SQL con la consulta y la conexión
                    using (var cmd = new SqlCommand(query, sql))
                    {
                        // Abrir la conexión
                        await sql.OpenAsync();

                        // Ejecutar la consulta y leer los resultados
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var objeto = new M_O_MAIL_OBJECTS
                                {
                                    MAILITM_FID = reader["MAILITM_FID"].ToString(),
                                    CUSTOMER_PHONE_NO = reader["CUSTOMER_PHONE_NO"].ToString(),
                                    COUNTRY_CD = reader["COUNTRY_CD"].ToString(),
                                    CUSTOMER_CITY = reader["CUSTOMER_CITY"].ToString(),
                                    LOCAL_EVENT_TYPE_NM =  reader["LOCAL_EVENT_TYPE_NM"].ToString(),
                                };

                                lista.Add(objeto);
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }

            return lista;
        }
    }
}
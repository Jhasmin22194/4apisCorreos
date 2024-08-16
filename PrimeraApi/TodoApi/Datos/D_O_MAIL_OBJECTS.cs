using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
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
                    string query =
                        @"
                        SELECT 
                            US.USER_NM,
                            M.MAIL_OBJECT_ID, 
                            M.MAIL_CLASS_CD,
                            MAX(DATA.value('(/DecData/@SNm)[1]', 'NVARCHAR(255)')) AS SNm,
                            MAX(DATA.value('(/DecData/@RNm)[1]', 'NVARCHAR(255)')) AS RNm,
                            MAX(DATA.value('(/DecData/@GWgt)[1]', 'DECIMAL(10, 3)')) AS GWgt,
                            MAX(DATA.value('(/DecData/@TrDat)[1]', 'DATETIME')) AS TrDat,
                            MAX(DATA.value('(/DecData/@TotCPVal)[1]', 'DECIMAL(10, 2)')) AS TotCPVal,
                            MAX(DATA.value('(/DecData/@SSta)[1]', 'NVARCHAR(255)')) AS SSta,
                            MAX(DATA.value('(/DecData/@SCtr)[1]', 'NVARCHAR(255)')) AS SCtr,
                            MAX(DATA.value('(/DecData/@RAdL1)[1]', 'NVARCHAR(255)')) AS RAdL1,
                            MAX(DATA.value('(/DecData/@RCty)[1]', 'NVARCHAR(255)')) AS RCty
                        FROM 
                            O_DECLARATIONS O
                        JOIN 
                            O_MAIL_OBJECTS M ON O.MAIL_OBJECT_PID = M.MAIL_OBJECT_PID
                        JOIN
                            O_DECLARATION_EVENTS OD ON O.DECLARATION_PID = OD.DECLARATION_PID
                        JOIN 
                            A_USERS US ON US.USER_CD = OD.USER_CD
                        GROUP BY 
                            US.USER_NM,
                            M.MAIL_OBJECT_ID, 
                            M.MAIL_CLASS_CD";

                    using (var cmd = new SqlCommand(query, sql))
                    {
                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var objeto = new M_O_MAIL_OBJECTS
                                {
                                    Usuario = reader["USER_NM"].ToString(),
                                    MailObjectId = reader["MAIL_OBJECT_ID"].ToString(),
                                    MailClassCd = reader["MAIL_CLASS_CD"].ToString(),
                                    SNm = reader["SNm"].ToString(),
                                    RNm = reader["RNm"].ToString(),
                                    GWgt = reader["GWgt"].ToString(),
                                    TrDat = reader["TrDat"].ToString(),
                                    TotCPVal = reader["TotCPVal"].ToString(),
                                    SSta = reader["SSta"].ToString(),
                                    SCtr = reader["SCtr"].ToString(),
                                    RAdL1 = reader["RAdL1"].ToString(),
                                    RCty = reader["RCty"].ToString(),
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

        public async Task<List<M_O_MAIL_OBJECTS>> FiltrarPorFechaYNombre(
            DateTime fecha,
            string nombre
        )
        {
            var lista = await MostrarDatos();
            return lista.FindAll(item =>
            {
                DateTime.TryParse(item.TrDat, out DateTime transactionDate);
                // Eliminar espacios en blanco del nombre ingresado y del usuario en la lista
                string nombreSinEspacios = nombre.Replace(" ", string.Empty);
                string usuarioSinEspacios = item.Usuario.Replace(" ", string.Empty);
                return transactionDate.Date == fecha.Date
                    && usuarioSinEspacios.Equals(
                        nombreSinEspacios,
                        StringComparison.OrdinalIgnoreCase
                    );
            });
        }
    }
}

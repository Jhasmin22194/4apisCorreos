using System.Data;
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

        public async Task<List<M_O_MAIL_OBJECTS>> MostrarDatosFiltrados(
            DateTime startDate,
            DateTime startDate2,
            string mailClassNm
        )
        {
            var lista = new List<M_O_MAIL_OBJECTS>();

            try
            {
                using (var sql = new SqlConnection(_cn.cadenaSQL()))
                {
                    string query =
                        @"
            SELECT L.MAILITM_FID,
CE.LOCAL_EVENT_TYPE_NM,
CON.LOCAL_COUNTRY_NM as ORI,
CONT.LOCAL_COUNTRY_NM AS DES,
CONVERT(VARCHAR(19),L.EVENT_GMT_DT, 120) AS EVENT_GMT_DT,
CONCAT(N.OFFICE_FCD,' - ',N.OFFICE_NM ) AS OFFICE_NM,
CONCAT(M.MAILITM_WEIGHT,' Kg') as MAILITM_WEIGHT,
C.MAIL_CLASS_NM,
U.USER_FID,
UPPER(INF.SIGNATORY_NM) as SIGNATORY_NM
FROM L_MAILITM_EVENTS L
JOIN L_MAILITMS M ON L.MAILITM_FID = M.MAILITM_FID
JOIN N_OWN_OFFICES N ON N.OWN_OFFICE_CD = L.EVENT_OFFICE_CD
JOIN C_MAIL_CLASSES C ON C.MAIL_CLASS_CD = M.MAIL_CLASS_CD
JOIN CT_EVENT_TYPES CE ON CE.EVENT_TYPE_CD=L.EVENT_TYPE_CD
JOIN CT_COUNTRIES CON ON CON.COUNTRY_CD = M.ORIG_COUNTRY_CD
LEFT JOIN CT_COUNTRIES CONT ON CONT.COUNTRY_CD=M.DEST_COUNTRY_CD
LEFT JOIN L_USERS U ON u.USER_PID = L.USER_PID 
LEFT JOIN L_MAILITM_DELIV_INFOS INF ON INF.MAILITM_PID = L.MAILITM_PID AND INF.SIGNATORY_NM IS NOT NULL
WHERE L.EVENT_TYPE_CD=37
            AND CONVERT(date, L.EVENT_GMT_DT) >= @startDate 
            AND CONVERT(date, L.EVENT_GMT_DT) <= @startDate2
            AND C.MAIL_CLASS_NM = @MailClassNm
            ORDER BY L.EVENT_GMT_DT";

                    using (var cmd = new SqlCommand(query, sql))
                    {
                        cmd.Parameters.Add(
                            new SqlParameter("@StartDate", SqlDbType.Date) { Value = startDate }
                        );
                        cmd.Parameters.Add(
                            new SqlParameter("@StartDate2", SqlDbType.Date) { Value = startDate2 }
                        );
                        cmd.Parameters.Add(
                            new SqlParameter("@MailClassNm", SqlDbType.NVarChar)
                            {
                                Value = mailClassNm
                            }
                        );

                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var objeto = new M_O_MAIL_OBJECTS
                                {
                                    mailitm_fid = reader["MAILITM_FID"].ToString().Trim(),
                                    local_event_type_nm = reader["LOCAL_EVENT_TYPE_NM"].ToString(),
                                    orig_country_cd = reader["ORI"].ToString(),
                                    dest_country_cd = reader["DES"].ToString(),
                                    event_gmt_dt = reader["EVENT_GMT_DT"].ToString(),
                                    office_nm = reader["OFFICE_NM"].ToString(),
                                    mailitm_weight = reader["MAILITM_WEIGHT"].ToString(),
                                    mail_class_nm = reader["MAIL_CLASS_NM"].ToString(),
                                    user_fid = reader["USER_FID"].ToString(),
                                    signatory_nm = reader["SIGNATORY_NM"].ToString(),
                                };

                                lista.Add(objeto);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Exception: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }

            return lista;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TodoApi.Datos;
using TodoApi.Modelo;

namespace TodoApi.Controllers
{
    //QUITANDO EL TOKEN
    /*
    
    http://localhost:5001/api/Autenticacion/Validar
    
    {
    "correo":"Correos",
    "clave":"AGBClp2020"
    }
   
    http://localhost:5254/api/L_USERS/filtrar?startDate=2023-08-17&startDate2=2023-08-23&mailClassNm=EMS

     */
    [Authorize]
    [ApiController]
    [Route("api/L_USERS")]
    public class O_MAIL_OBJECTScontroller : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public O_MAIL_OBJECTScontroller(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Endpoint para obtener los datos filtrados por fecha y tipo de correspondencia
        [HttpGet("filtrar")]
        public async Task<ActionResult<List<M_O_MAIL_OBJECTS>>> GetFiltered([FromQuery] DateTime startDate,[FromQuery] DateTime startDate2, [FromQuery] string mailClassNm)
        {
            var funcion = new D_O_MAIL_OBJECTS(_configuration);
            var lista = await funcion.MostrarDatosFiltrados(startDate,startDate2, mailClassNm);
            return lista;
        }
    }
}



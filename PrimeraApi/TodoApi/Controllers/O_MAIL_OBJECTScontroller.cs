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
    /*
    http://localhost:5002/api/Autenticacion/Validar
    
    {
    "correo":"Correos",
    "clave":"AGBClp2020"
    }
    http://localhost:5002/api/O_MAIL_OBJECTS/filtrar
    {
    "Fecha": "2023-09-29",
    "Nombre": "GICELA MENDEZ TARQUI"
    }
    
    */
    [Authorize]
    [ApiController]
    [Route("api/O_MAIL_OBJECTS")]
    public class O_MAIL_OBJECTScontroller : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public O_MAIL_OBJECTScontroller(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<List<M_O_MAIL_OBJECTS>>> Get()
        {
            var funcion = new D_O_MAIL_OBJECTS(_configuration);
            var lista = await funcion.MostrarDatos();
            return lista;
        }

        [HttpPost]
        [Route("filtrar")]
        public async Task<ActionResult<List<M_O_MAIL_OBJECTS>>> FiltrarPorFechaYNombre(
            [FromBody] FiltrarRequest request
        )
        {
            var funcion = new D_O_MAIL_OBJECTS(_configuration);
            var lista = await funcion.FiltrarPorFechaYNombre(request.Fecha, request.Nombre);
            return lista;
        }

        public class FiltrarRequest
        {
            public DateTime Fecha { get; set; }
            public string Nombre { get; set; }
        }
        //73017637
    }
}



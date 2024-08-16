
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Datos;
using TodoApi.Modelo;


namespace TodoApi.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("")]
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
    }
}

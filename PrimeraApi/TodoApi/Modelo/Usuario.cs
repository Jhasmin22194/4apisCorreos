namespace TodoApi.Modelo
{
    public class Usuario
    {
        public string correo { get; set; }
        public string clave { get; set; }

        // Constructor para inicializar las propiedades
        public Usuario()
        {
            correo = string.Empty;
            clave = string.Empty;
        }
    }
}


using CapaDatos.Models;

namespace CapaDatos.Negocio
{
    public class UserSession
    {
        public string Email { get; set; }
        public string Rol { get; set; }
        public string Password { get; set; }
        public Administrador Administrador { get; set; } = new();
        public SupervisorLinea SupervisorLinea { get; set; } = new();
        public SupervisorCalidad SupervisorCalidad { get; set; } = new();

        public JornadaLaboral JornadaLaboralActiva { get; set; } = new();
    } 
}

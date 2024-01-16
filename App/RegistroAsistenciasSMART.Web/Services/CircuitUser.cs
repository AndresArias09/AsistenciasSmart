using RegistroAsistenciasSMART.Web.Authentication;

namespace RegistroAsistenciasSMART.Web.Services
{
    public class CircuitUser
    {
        public UserSession usuario { get; set; }
        public string CircuitId { get; set; }
    }
}

using RegistroAsistenciasSMART.Web.Authentication;
using System.Collections.Concurrent;

namespace RegistroAsistenciasSMART.Web.Services
{
    public interface ICircuitUserService
    {
        ConcurrentDictionary<string, CircuitUser> Circuits { get; }

        event UserEventHandler UserConnected;
        event UserEventHandler UserDisconnected;

        void Connect(string CircuitId, UserSession user);
        void Disconnect(string CircuitId);
    }
}

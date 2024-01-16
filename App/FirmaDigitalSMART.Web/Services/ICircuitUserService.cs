using FirmaDigitalSMART.Web.Authentication;
using System.Collections.Concurrent;

namespace FirmaDigitalSMART.Web.Services
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

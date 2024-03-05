using RegistroAsistenciasSMART.Web.Authentication;
using System.Collections.Concurrent;

namespace RegistroAsistenciasSMART.Web.Services
{
    /// <summary>
    /// Implementación de la interfaz <see cref="ICircuitUserService"/>
    /// </summary>
    public class CircuitUserService : ICircuitUserService
    {
        public ConcurrentDictionary<string, CircuitUser> Circuits { get; private set; }
        public event UserEventHandler UserConnected;
        public event UserEventHandler UserDisconnected;

        private readonly ILogger<CircuitUserService> _logger;

        public CircuitUserService(ILogger<CircuitUserService> logger)
        {
            _logger = logger;
            Circuits = new ConcurrentDictionary<string, CircuitUser>();
        }
        /// <summary>
        /// Invoca el evento <see cref="UserConnected"/> cuando se conecta un usuario
        /// </summary>
        /// <param name="user">Varible de sesión del usuario recién conectado</param>
        void OnConnectedUser(UserSession user)
        {
            try
            {
                //_logger.LogInformation($"Se dispara evento de usuario conectado {user.UserName}");

                var args = new UserEventArgs();
                args.usuario = user;
                UserConnected?.Invoke(this, args);
            }
            catch (Exception exe)
            {
                _logger.LogError(exe, $"Error al invocar evento de usuario conectado {user.UserName}");
            }

        }
        /// <summary>
        /// Invoca el evento <see cref="UserDisconnected"/> cuando se desconecta un usuario
        /// </summary>
        /// <param name="user">Variable de sesión del usuario recién desconectado</param>
        void OnDisconnectedUser(UserSession user)
        {
            try
            {
                //_logger.LogInformation($"Se dispara evento de usuario desconectado {user.UserName}");

                var args = new UserEventArgs();
                args.usuario = user;
                UserDisconnected?.Invoke(this, args);
            }
            catch (Exception exe)
            {
                _logger.LogError(exe, $"Error al invocar evento de usuario desconectado");
            }

        }

        public void Connect(string CircuitId, UserSession user)
        {
            try
            {
                //_logger.LogInformation($"Se procesa conexión {user.UserName}");

                if (Circuits.ContainsKey(CircuitId))
                    Circuits[CircuitId].usuario = user;
                else
                {
                    var circuitUser = new CircuitUser();
                    circuitUser.usuario = user;
                    circuitUser.CircuitId = CircuitId;
                    Circuits[CircuitId] = circuitUser;
                }
                OnConnectedUser(user);
            }
            catch (Exception exe)
            {
                _logger.LogError(exe, $"Error al procesar conexión {user.UserName}");
            }

        }

        public void Disconnect(string CircuitId)
        {
            try
            {
                //_logger.LogInformation($"Se procesa desconexión {CircuitId}");

                CircuitUser circuitRemoved;
                Circuits.TryRemove(CircuitId, out circuitRemoved);
                if (circuitRemoved != null)
                {
                    OnDisconnectedUser(circuitRemoved?.usuario);
                }
            }
            catch (Exception exe)
            {
                _logger.LogError(exe, $"Error al procesar desconexión {CircuitId}");
            }

        }
    }
}

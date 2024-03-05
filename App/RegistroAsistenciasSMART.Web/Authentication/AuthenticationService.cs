using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;
using RegistroAsistenciasSMART.Web.Services;

namespace RegistroAsistenciasSMART.Web.Authentication
{
    public class AuthenticationService
    {
        private readonly ICircuitUserService _circuitUserServer;
        private readonly CircuitHandler _blazorCircuitHandler;
        private readonly CustomAuthenticationStateProvider _authStateProvider;

        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService
        (
            ICircuitUserService circuitUserServer,
            CircuitHandler blazorCircuitHandler,
            AuthenticationStateProvider authStateProvider,

            ILogger<AuthenticationService> logger
        )
        {
            _circuitUserServer = circuitUserServer;
            _blazorCircuitHandler = blazorCircuitHandler;
            _authStateProvider = (CustomAuthenticationStateProvider)authStateProvider;

            _logger = logger;
        }

        public async Task LimpiarSesion()
        {
            try
            {
                //Vaciar el estado de autenticación actual
                await _authStateProvider.UpdateAuthenticationState(null);

                //Desconectar el circuito
                desconectarCircuito();
            }
            catch (Exception exe)
            {
                _logger.LogError(exe, "Error al limpiar sesión");
            }
        }

        public void conectarCircuito(UserSession session)
        {
            CircuitHandlerService handler = (CircuitHandlerService)_blazorCircuitHandler;
            _circuitUserServer.Connect(handler.CirtuidId, session);
        }

        public void desconectarCircuito()
        {
            CircuitHandlerService handler = (CircuitHandlerService)_blazorCircuitHandler;
            _circuitUserServer.Disconnect(handler.CirtuidId);
        }

        public async Task<UserSession> getActualSession()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();

            var user = authState.User;

            if (!user.Identity.IsAuthenticated)
            {
                return null;
            }

            var customAuthStateProvider = (CustomAuthenticationStateProvider)_authStateProvider;
            UserSession userSession = await customAuthStateProvider.GetActualUser();

            return userSession;
        }

        public async Task updateActualSession(UserSession session)
        {
            await _authStateProvider.UpdateAuthenticationState(session);
        }
    }
}

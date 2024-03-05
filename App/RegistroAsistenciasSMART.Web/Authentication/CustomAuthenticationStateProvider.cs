using RegistroAsistenciasSMART.Model.Models.Configuracion.Perfilamiento;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace RegistroAsistenciasSMART.Web.Authentication
{
    /// <summary>
    /// Proveedor personalización de autenticación para el manejo de las sesiones de usuario del sistema
    /// </summary>
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        /// <summary>
        /// Servicio que permite el guardado de objetos en almacenamiento del navegador de manera local
        /// </summary>
        private readonly ProtectedLocalStorage _localStorage;
        /// <summary>
        /// Claim anónima
        /// </summary>
        private ClaimsPrincipal _anonumous = new ClaimsPrincipal(new ClaimsIdentity());

        private readonly ILogger<CustomAuthenticationStateProvider> _logger;

        public CustomAuthenticationStateProvider(ProtectedLocalStorage sessionStorage, ILogger<CustomAuthenticationStateProvider> logger)
        {
            _localStorage = sessionStorage;
            _logger = logger;
        }
        /// <summary>
        ///  Obtiene el estado de autenticación actual del usuario. Para determinar el estado de la autenticación se consulta el almacenamiento local del navegador
        /// </summary>
        /// <returns><see cref="AuthenticationState"/></returns>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userSessionStorageResult = await _localStorage.GetAsync<UserSession>("UserSession");
                var userSession = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;

                if (userSession == null)
                    return await Task.FromResult(new AuthenticationState(_anonumous));

                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier,userSession.UserName),

                }, "CustomAuth"));
                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch (Exception exe)
            {
                return await Task.FromResult(new AuthenticationState(_anonumous));
            }
        }
        /// <summary>
        /// Actualiza el estado de autenticación de la sesión actual
        /// </summary>
        /// <param name="userSession">Objeto <see cref="UserSession"/> que contiene la información que será guardada en almacenamiento local del navegador</param>
        public async Task UpdateAuthenticationState(UserSession userSession)
        {
            ClaimsPrincipal claimsPrincipal;

            if (userSession != null)
            {
                await _localStorage.SetAsync("UserSession", userSession);

                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier,userSession.UserName),
                }));
            }
            else
            {
                await _localStorage.DeleteAsync("UserSession");
                claimsPrincipal = _anonumous;
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
        /// <summary>
        /// Obtiene la sesión de usuario actual
        /// </summary>
        /// <returns>Objeto <see cref="UserSession"/> que contiene todos los datos de la sesión actual</returns>
        public async Task<UserSession> GetActualUser()
        {
            var authState = await GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity.IsAuthenticated)
            {
                var result = await _localStorage.GetAsync<UserSession>("UserSession");
                UserSession userSession = result.Success ? result.Value : new UserSession();
                return userSession;
            }
            else
            {
                return new UserSession();
            }
        }

    }
}

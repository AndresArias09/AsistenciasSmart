﻿using RegistroAsistenciasSMART.Model.Models.Configuracion.Perfilamiento;
using RegistroAsistenciasSMART.Services.Interfaces.Configuracion.Perfilamiento;
using RegistroAsistenciasSMART.Web.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace RegistroAsistenciasSMART.Web.Authorization
{
    public class AuthorizationService : IDisposable
    {
        private readonly NavigationManager _navigationManager;
        private readonly IJSRuntime _jsRuntime;

        public AuthorizationService()
        {
                
        }

        public AuthorizationService(
            NavigationManager navigationManager,
            IJSRuntime jsRuntime)
        {
            _navigationManager = navigationManager;
            _jsRuntime = jsRuntime;


            _navigationManager.LocationChanged += InfoCliente;
        }

        private async void InfoCliente(object sender, LocationChangedEventArgs e)
        {
            await _jsRuntime.InvokeVoidAsync("getClientInfo");
        }

        void IDisposable.Dispose()
        {
            // Unsubscribe from the event when our component is disposed
            _navigationManager.LocationChanged -= InfoCliente;
        }
    }


}

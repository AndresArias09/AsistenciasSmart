using Microsoft.AspNetCore.Components.Server.Circuits;
using RegistroAsistenciasSMART.Data.Connection;
using RegistroAsistenciasSMART.Services.Interfaces.Auditoria;
using RegistroAsistenciasSMART.Services.Interfaces.Configuracion.Perfilamiento;
using RegistroAsistenciasSMART.Services.Interfaces.Utilidades;
using RegistroAsistenciasSMART.Services.Services.Auditoria;
using RegistroAsistenciasSMART.Services.Services.Configuracion.Perfilamiento;
using RegistroAsistenciasSMART.Services.Services.Utilidades;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Serilog;
using Serilog.Events;
using RegistroAsistenciasSMART.Web.Services;
using RegistroAsistenciasSMART.Web.Authentication;
using RegistroAsistenciasSMART.Web.Helpers;
using RegistroAsistenciasSMART.Web.Authorization;
using RegistroAsistenciasSMART.Services.Services.Colaboradores;
using RegistroAsistenciasSMART.Services.Interfaces.Colaboradores;


//Logs
string ruta_logger = Directory.GetCurrentDirectory() +
    "\\Log\\LogEventos_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".txt";

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File(ruta_logger)
    .WriteTo.Console()
    .CreateLogger();

Log.Information("Starting up the application");


try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    // Add services to the container.
    builder.Services.AddAuthenticationCore();
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor(options =>
    {
        options.DetailedErrors = false;
        options.DisconnectedCircuitMaxRetained = 100;
        options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(3);
        options.JSInteropDefaultCallTimeout = TimeSpan.FromMinutes(1);
        options.MaxBufferedUnacknowledgedRenderBatches = 10;
    });

    builder.Services.AddServerSideBlazor().AddHubOptions(options =>
    {
        options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
        options.EnableDetailedErrors = false;
        options.HandshakeTimeout = TimeSpan.FromSeconds(15);
        options.KeepAliveInterval = TimeSpan.FromSeconds(15);
        options.MaximumParallelInvocationsPerClient = 1;
        options.MaximumReceiveMessageSize = 102400000;
        options.StreamBufferCapacity = 10;
    });

    builder.Services.AddSingleton<ICircuitUserService, CircuitUserService>();
    builder.Services.AddScoped<CircuitHandler>((sp) => new CircuitHandlerService(sp.GetRequiredService<ICircuitUserService>()));


    //INYECCI�N DE DEPENDENCIAS - SERVICIOS

    //FUNDAMENTALES


    builder.Services.AddScoped<ProtectedSessionStorage>();
    builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
    builder.Services.AddScoped<AuthorizationService>();

    builder.Services.AddScoped<IAuditoriaService, AuditoriaService>();

    builder.Services.AddScoped<IEncryptService, EncryptService>();
    builder.Services.AddScoped<IEmailService, EmailService>();
    builder.Services.AddScoped<IUserService, UserService>();


    builder.Services.AddScoped<IColaboradorService, ColaboradorService>();


    //DEFINICI�N DE LA CONEXI�N A BASE DE DATOS
    var sqlConnectionConfiguration = new SqlConfiguration(builder.Configuration.GetConnectionString("DefaultConnection"));
    builder.Services.AddSingleton(sqlConnectionConfiguration);

    IJSRuntimeExtension.setAuditoriaService(new AuditoriaService(sqlConnectionConfiguration));

    //CommandTimeout Dapper
    Dapper.SqlMapper.Settings.CommandTimeout = 10000;

    builder.Services.AddHsts(options =>
    {
        options.Preload = true;
        options.IncludeSubDomains = true;
        options.MaxAge = TimeSpan.FromDays(181);
    });


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseResponseCompression();
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }


    app.Use(async (context, next) =>
    {
        context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
        context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Add("Referrer-Policy", "same-origin");
        context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
        context.Response.Headers.Add("SameSite", "Strict");
        //context.Response.Headers.Add("Clear-Site-Data", @"""cache""");
        await next();
    });

    app.UseHttpsRedirection();
    app.UseHsts();

    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();

    app.MapControllers();
    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");

    app.Run();
}
catch (Exception exe)
{
    Log.Fatal(exe, "There was a problem starting the application");
    return;
}
finally
{
    Log.CloseAndFlush();
}



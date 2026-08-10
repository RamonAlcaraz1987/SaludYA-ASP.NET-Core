using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Text;
using SaludYa.Models;
using SaludYa.Controllers;

var builder = WebApplication.CreateBuilder(args);

// ─── Servicios ────────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ─── Repositorios (inyección de dependencias) ─────────────────────────────────
builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
builder.Services.AddScoped<IRepositorioCentroSalud, RepositorioCentroSalud>();
builder.Services.AddScoped<IRepositorioEspecialista, RepositorioEspecialista>();
builder.Services.AddScoped<IRepositorioCronograma, RepositorioCronograma>();
builder.Services.AddScoped<IRepositorioNovedadDiaria, RepositorioNovedadDiaria>();
builder.Services.AddScoped<IRepositorioVacunatorio, RepositorioVacunatorio>();
builder.Services.AddScoped<IRepositorioCatalogoVacunas, RepositorioCatalogoVacunas>();
builder.Services.AddScoped<IRepositorioVacunaDisponible, RepositorioVacunaDisponible>();
builder.Services.AddScoped<IRepositorioAuditoria, RepositorioAuditoria>();
builder.Services.AddScoped<IRepositorioDeviceToken, RepositorioDeviceToken>();
builder.Services.AddHttpClient<FirebaseService>();

// ─── Autenticación: Cookies (panel web) + JWT (app móvil) ────────────────────
var secretKey = builder.Configuration["TokenAuthentication:SecretKey"] ?? "SaludYaClaveSecretaMuyLarga2026!";

builder.Services.AddAuthentication(options =>
    {
        // El panel web (Razor Pages) usa Cookies por defecto.
        // Los controllers de la API especifican JwtBearer explícitamente.
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["TokenAuthentication:Issuer"] ?? "SaludYa",
            ValidAudience = builder.Configuration["TokenAuthentication:Audience"] ?? "SaludYaAPI",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

// ─── Políticas de autorización por rol ───────────────────────────────────────
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SoloSuperadmin", policy =>
        policy.RequireClaim(System.Security.Claims.ClaimTypes.Role, "superadmin"));

    options.AddPolicy("ResponsableOSuperadmin", policy =>
        policy.RequireClaim(System.Security.Claims.ClaimTypes.Role, "responsable", "superadmin"));
});

// ─── CORS (para el cliente Android y web) ────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("SaludYaPolicy", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// ─── Cultura invariante: los inputs numéricos (type="number", ej. Latitud/
//     Longitud) siempre mandan el valor con punto decimal desde el navegador.
//     Si el servidor corre con configuración regional es-AR (coma decimal),
//     el model binder falla en silencio y esas propiedades llegan en null.
//     Esto fuerza que el binding numérico siempre interprete punto decimal,
//     sin afectar los textos/fechas en español que arma Razor. ─────────────────
var culturaInvariante = new CultureInfo("en-US");
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(culturaInvariante),
    SupportedCultures = new[] { culturaInvariante },
    SupportedUICultures = new[] { culturaInvariante }
});

// ─── Pipeline ─────────────────────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("SaludYaPolicy");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapRazorPages();

app.Run();
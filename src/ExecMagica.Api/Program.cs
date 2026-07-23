using ExecMagica.Application.Interfaces;
using ExecMagica.Application.Services;
using ExecMagica.Infrastructure.Data;
using ExecMagica.Infrastructure.Identity;
using ExecMagica.Infrastructure.Rendering;
using ExecMagica.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.FileProviders;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));
builder.Services.AddOpenApi();

// Database.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// Identity (user/role management over EF Core).
builder.Services
    .AddIdentityCore<ApplicationUser>(options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.User.RequireUniqueEmail = true;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

// JWT settings + authentication.
var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()!;
builder.Services.AddSingleton(jwtOptions);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
            ValidateLifetime = true,
        };
    });

builder.Services.AddAuthorization();

// Application/Infrastructure services.
builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<IDeckRepository, DeckRepository>();
builder.Services.AddScoped<IDeckService, DeckService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddSingleton<IRenderService, SvgCardRenderService>();
// Persist DataProtection keys to a mounted volume in production.
var keysPath = builder.Configuration["DataProtectionKeysPath"];
if (!string.IsNullOrWhiteSpace(keysPath))
{
    Directory.CreateDirectory(keysPath);
    builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(keysPath));
}

var app = builder.Build();

// Apply migrations and seed data + identity on startup.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var db = services.GetRequiredService<AppDbContext>();
    await DbInitializer.SeedAsync(db);

    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await IdentityInitializer.SeedAsync(
        userManager,
        roleManager,
        builder.Configuration["SeedAdmin:Email"],
        builder.Configuration["SeedAdmin:Password"]);
}

app.MapOpenApi();
app.MapScalarApiReference();

// Behind Caddy: trust forwarded scheme/host (Caddy terminates TLS).
var forwarded = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
};
forwarded.KnownIPNetworks.Clear();
forwarded.KnownProxies.Clear();
app.UseForwardedHeaders(forwarded);

app.UseAuthentication();
app.UseAuthorization();

// Static templates/font (baked into the image).
app.UseStaticFiles();

// Card art — served from a directory that is a mounted volume in production.
var artPath = app.Configuration["ArtPath"]
    ?? Path.Combine(app.Environment.ContentRootPath, "art");
Directory.CreateDirectory(artPath);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(artPath),
    RequestPath = "/art",
});

app.MapControllers();

app.Run();

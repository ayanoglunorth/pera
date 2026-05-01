using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens; 
using Microsoft.OpenApi.Models;
using Pera.Business.Abstract;   // Interface'ler için
using Pera.Business.Concrete;   // Class'lar için
using Pera.DataAccess;
using Pera.DataAccess.Abstract;
using Pera.DataAccess.Concrete;
using Pera.DataAccess.Concrete.EntityFramework;
using Pera.Entity.Entities;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Repository Kaydý: "Veritabaný iþlerini bu sýnýf yapar"

builder.Services.AddScoped<IDersService, DersService>();
builder.Services.AddScoped<IDersRepository, DersRepository>();


// Auth Servisi Kaydý
builder.Services.AddScoped<IAuthService, AuthService>();

// Bu satýr, DbContext'e o aradýðý "options" parametresini gönderir.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.

builder.Services.AddIdentity<AppUser, IdentityRole>() // <--- BURASI ÇOK ÖNEMLÝ: IdentityRole EKLENDÝ
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddCors(options =>
{
    options.AddPolicy("IzinVerilenler", policy =>
    {
        policy.AllowAnyOrigin()   // Þimdilik herkese izin ver (Test için)
              .AllowAnyHeader()   // Her türlü baþlýðý kabul et
              .AllowAnyMethod();  // GET, POST, PUT, DELETE hepsine izin ver
    });
});

// --- JWT DOÐRULAMA AYARLARI ---
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        // Appsettings.json'daki deðerlerle AYNI olmalý
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecurityKey"]))
    };
});

builder.Services.AddScoped<IDenemeRepository, DenemeRepository>();
builder.Services.AddScoped<IDenemeService, DenemeService>();
builder.Services.AddScoped<IMesajService, MesajService>();
builder.Services.AddScoped<IMesajRepository, MesajRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pera API", Version = "v1" });

    // Kilit simgesini ve Token giriþini aktif eden ayar
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Örnek: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseCors("IzinVerilenler");
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();

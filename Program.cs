using Microsoft.EntityFrameworkCore;
using DesafioBackEndManipulae.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

// Criar builder da aplicação
var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// 🔹 Configuração do Banco de Dados (SQLite)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=videos.db"));

// 🔹 Configuração do JWT
var key = Encoding.ASCII.GetBytes(config["JwtSettings:Secret"]);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config["JwtSettings:Issuer"],
            ValidAudience = config["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();

// 🔹 Correção: Registrar Interfaces com suas Implementações
builder.Services.AddScoped<IVideoRepository, VideoRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// 🔹 Registrar os Serviços
builder.Services.AddScoped<VideoService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();

// 🔹 Configurar a API do YouTube
var youtubeApiKey = builder.Configuration["YouTubeApi:ApiKey"];
builder.Services.AddSingleton(new YouTubeService(youtubeApiKey));

// 🔹 Adicionar Controllers e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DesafioBackEndManipulae - API", Version = "v1" });

    // Configuração da autenticação JWT no Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Entre com o token JWT no formato: Bearer SEU_TOKEN_AQUI",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
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
            new string[] { }
        }
    });
});

var app = builder.Build();

// 🔹 Middleware de Tratamento de Erros Global
app.UseMiddleware<ExceptionHandlingMiddleware>();

// 🔹 Ativar Autenticação e Autorização
app.UseAuthentication();
app.UseAuthorization();

// 🔹 Configuração do Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

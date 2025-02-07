using Microsoft.EntityFrameworkCore;
using DesafioBackEndManipulae.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=videos.db"));

// Configuração do JWT
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

builder.Services.AddScoped<VideoRepository>();
builder.Services.AddScoped<VideoService>();

builder.Services.AddAuthorization();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var youtubeApiKey = builder.Configuration["YouTubeApi:ApiKey"];
builder.Services.AddSingleton(new YouTubeService(youtubeApiKey));


var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>(); // Adiciona um tratamento global de erros

app.UseAuthentication(); // Habilita autenticação JWT
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(); 
app.UseHttpsRedirection();
app.MapControllers();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}



app.Run();


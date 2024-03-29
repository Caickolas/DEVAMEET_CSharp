using DEVAMEET_CSharp;
using DEVAMEET_CSharp.Hubs;
using DEVAMEET_CSharp.Models;
using DEVAMEET_CSharp.Repository;
using DEVAMEET_CSharp.Repository.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectstring = builder.Configuration.GetConnectionString("DefaulConnectString");
builder.Services.AddDbContext<DevameetContext>(option => option.UseSqlServer(connectstring));

builder.Services.AddScoped<IUserRepository, UserRepositoryImpl>();
builder.Services.AddScoped<IMeetRepository, MeetRepositoryImpl>();
builder.Services.AddScoped<IRoomRepository, RoomRepositoryImpl>();
builder.Services.AddScoped<IMeetObjectsRepository, MeetObjectsRepositoryImpl>();

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientPermission", policy => 
        {
            policy.AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://localhost:3000") // trocar caso hospede o site.
            .AllowCredentials();
        });
});

var jwtsettings = builder.Configuration.GetRequiredSection("JWT").Get<JWTKey>();

var secretKey = Encoding.ASCII.GetBytes(jwtsettings.SecretKey);
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(authentication =>
{
    authentication.RequireHttpsMetadata = false;
    authentication.SaveToken = true;
    authentication.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("ClientPermission");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<RoomHub>("/roomHub");

app.Run();

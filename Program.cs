using FiTrack.Api.Data;
using FiTrack.Api.Services.Auth;
using FiTrack.Api.Services.Interfaces;
using FiTrack.Api.Services;
using Microsoft.EntityFrameworkCore;
using FiTrack.Api.Models.Users;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FiTrackDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FiTrackDb")));

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddScoped<IWeightLogService, WeightLogService>();

builder.Services.AddScoped<IFoodService, FoodService>();

builder.Services.AddScoped<IMealService, MealService>();

builder.Services.AddScoped<IFoodLogService, FoodLogService>();

builder.Services.AddScoped<IExerciseCatalogService, ExerciseCatalogService>();

builder.Services.AddScoped<IWorkoutDayService, WorkoutDayService>();

builder.Services.AddScoped<IWorkoutSessionService, WorkoutSessionService>();

builder.Services.AddScoped<IActivityTypeService, ActivityTypeService>();

builder.Services.AddScoped<IActivityLogService, ActivityLogService>();

builder.Services.AddScoped<ITodayService, TodayService>();

var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSettings["Key"]
    ?? throw new InvalidOperationException("JWT Key is not configured.");
var jwtIssuer = jwtSettings["Issuer"]
    ?? throw new InvalidOperationException("JWT Issuer is not configured.");
var jwtAudience = jwtSettings["Audience"]
    ?? throw new InvalidOperationException("JWT Audience is not configured.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey))
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
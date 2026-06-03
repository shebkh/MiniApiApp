using EventsApi.Models;
using EventsApi.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var mvcBuilder = builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile));
mvcBuilder.AddFluentValidation(fv =>
    fv.RegisterValidatorsFromAssemblyContaining<CreateEventValidator>());



// 1. Bind settings classes
var jwtSettings = builder.Configuration
    .GetSection("JwtSettings").Get<JwtSettings>()!;
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// 2. Add ASP.NET Identity
builder.Services
    .AddIdentity<AppUser, IdentityRole>(options => {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireUppercase = false;
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedEmail = true;
    })
     .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders(); // enables email confirmation tokens

// 3. Add JWT Authentication
builder.Services
    .AddAuthentication(options => {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        };
    });

builder.Services.AddAuthorization();

// 4. Register our custom services (we'll create these next)
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();
app.UseAuthentication(); // reads the token
app.UseAuthorization();  // checks permissions — must come AFTER
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles(); // enables serving files from wwwroot/

// Create the uploads folder if it doesn't exist
var uploadsPath = Path.Combine(builder.Environment.WebRootPath ??
    "wwwroot", "uploads");
Directory.CreateDirectory(uploadsPath);

app.MapControllers();

app.Run();

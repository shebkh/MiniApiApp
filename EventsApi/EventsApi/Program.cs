using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

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


var app = builder.Build();

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

app.UseAuthorization();

app.MapControllers();

app.Run();

using LearningHub.Application.Interfaces.Persistence;
using LearningHub.Application.Services;
using LearningHub.Infrastructure.Persistence; 
using Microsoft.EntityFrameworkCore;
using LearningHub.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using LearningHub.Application.Interfaces.Services;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<LearningHubDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
// Services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IStudentService, StudentService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
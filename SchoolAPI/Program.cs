using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Repositories;
using SchoolAPI.Repositories.Course;
using SchoolAPI.Repositories.Student;
using SchoolAPI.Repositories.Enrollment;
using SchoolAPI.Services.Course;
using SchoolAPI.Services.Student;
using SchoolAPI.Services.Enrollment;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add repositories
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();

// Add services
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();

// AutoMapper
builder.Services.AddAutoMapper(cfg => { }, typeof(Program));

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Add Authorization (fixar felet du fick)
builder.Services.AddAuthorization();

// Add Controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
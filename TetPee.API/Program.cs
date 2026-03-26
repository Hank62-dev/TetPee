using Microsoft.EntityFrameworkCore;
using TetPee.Api.Extensions;
using TetPee.Api.Middlewares;
using TetPee.Repository;
using TetPee.Service.Category;
using TetPee.Service.Identity;
using TetPee.Service.JwtService;
using TetPee.Service.Seller;
using TetPee.Service.User;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(); //cho dùng API controller [Apicontroller]
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//tạo UI test API(Swagger)

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddJwtService(builder.Configuration);
builder.Services.AddSwaggerServices();
//kết noi database (PostgreSQL)

builder.Services.AddScoped<IService, Service>();
builder.Services.AddScoped<IServiceCate, ServiceCate>();
builder.Services.AddScoped<IServiceSeller, ServiceSeller>();
builder.Services.AddScoped<IServiceIden, ServiceIden>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<TetPee.Service.Product.IService, TetPee.Service.Product.Service>();
builder.Services.AddScoped<TetPee.Service.MediaService.IService, TetPee.Service.CloudinaryService.Service>();
// đăng ki Service (Dependency Injection)

builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();



var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
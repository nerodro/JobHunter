using CategoryAPI.ServiceGrpc;
using CategoryRepository;
using CategoryRepository.CategoryLogic;
using CategoryService.CategoryService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// добавляем контекст ApplicationContext в качестве сервиса в приложение
builder.Services.AddDbContext<CategoryContext>(options => options.UseNpgsql(connection));

builder.Services.AddScoped(typeof(ICategoryLogic<>), typeof(CategoryLogic<>));
builder.Services.AddTransient<ICategoryService, CategoryServices>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<GrpcCategory>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

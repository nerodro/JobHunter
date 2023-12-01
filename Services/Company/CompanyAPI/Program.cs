using CompanyAPI.RabbitMq;
using CompanyAPI.ServiceGrpc;
using CompanyRepository;
using CompanyRepository.CompanyLogic;
using CompanyService.CompanyService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// добавляем контекст ApplicationContext в качестве сервиса в приложение
builder.Services.AddDbContext<CompanyContext>(options => options.UseNpgsql(connection));

builder.Services.AddScoped(typeof(ICompanyLogic<>), typeof(CompanyLogic<>));
builder.Services.AddTransient<ICompanyService, CompanyServices>();

builder.Services.AddScoped<ICompanyProducer, CompanyProducer>();

builder.Services.AddScoped<CategoryRpc>();
builder.Services.AddScoped<LocationRpc>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGrpcService<CategoryRpc>();
app.MapGrpcService<LocationRpc>();

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

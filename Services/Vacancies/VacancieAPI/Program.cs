using Microsoft.EntityFrameworkCore;
using ResponseRepository.ResponseLogic;
using VacancieAPI.ServiceGrpc;
using VacancieAPI.VacancieRpc;
using VacancieRepository;
using VacancieRepository.ResponseLogic;
using VacancieRepository.VacancieLogic;
using VacancieService.ResponseService;
using VacancieService.VacancieService;
using VacancieService.VacancyService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// добавляем контекст ApplicationContext в качестве сервиса в приложение
builder.Services.AddDbContext<VacancyContext>(options => options.UseNpgsql(connection));

builder.Services.AddScoped(typeof(IVacancieLogic<>), typeof(VacancieLogic<>));
builder.Services.AddScoped(typeof(IResponseLogic<>), typeof(ResponseLogic<>));

builder.Services.AddScoped<LocationRpc>();
builder.Services.AddScoped<CompanyRpc>();
builder.Services.AddScoped<CvRpc>();

builder.Services.AddTransient<IVacancieService, VacancyServices>();
builder.Services.AddTransient<IResponseService, ResponseService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<LocationRpc>();
app.MapGrpcService<CompanyRpc>();
app.MapGrpcService<CvRpc>();
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

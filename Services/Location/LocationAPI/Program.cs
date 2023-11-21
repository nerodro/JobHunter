using LocationRepository;
using LocationRepository.CitryLogic;
using LocationRepository.CountryLogic;
using Microsoft.EntityFrameworkCore;
using LocationService.CityService;
using LocationService.CountryService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// ��������� �������� ApplicationContext � �������� ������� � ����������
builder.Services.AddDbContext<LocationContext>(options => options.UseNpgsql(connection));

builder.Services.AddScoped(typeof(ICityLogic<>), typeof(CityLogic<>));
builder.Services.AddScoped(typeof(ICountryLogic<>), typeof(CountryLogic<>));

builder.Services.AddTransient<ICityService, CityService>();
builder.Services.AddTransient<ICountryService, CountryService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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

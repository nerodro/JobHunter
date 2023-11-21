using Microsoft.EntityFrameworkCore;
using UserAPI.ServiceGrpc;
using UserRepository.CvLogic;
using UserRepository.LanguageLogic;
using UserRepository.UserContext;
using UserRepository.UserLogic;
using UserService.CvService;
using UserService.LanguageService;
using UserService.UserService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// ��������� �������� ApplicationContext � �������� ������� � ����������
builder.Services.AddDbContext<UserDbContext>(options => options.UseNpgsql(connection));

builder.Services.AddScoped(typeof(IUserLogic<>), typeof(UserLogic<>));
builder.Services.AddScoped(typeof(ICvLogic<>), typeof(CvLogic<>));
builder.Services.AddScoped(typeof(ILanguageLogic<>), typeof(LanguageLogic<>));

builder.Services.AddScoped<CategoryRpc>();

builder.Services.AddTransient<IUserService, UserServices>();
builder.Services.AddTransient<ICvService, CvService>();
builder.Services.AddTransient<ILanguageService, LanguageService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<CategoryRpc>();

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

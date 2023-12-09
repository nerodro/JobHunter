using MassTransit;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using ResponseAPI.RabbitMq;
using ResponseRepository.ResponseLogic;
using UserRepository.Login;
using UserRepository.Registration;
using UserService.LoginService;
using UserService.RegistrationService;
using VacancieAPI.RabbitMq;
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
builder.Services.AddDbContext<VacancyContext>(options => options.UseNpgsql(connection), ServiceLifetime.Singleton);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie(options =>
              {
                  options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                  options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
              });


builder.Services.AddScoped(typeof(IVacancieLogic<>), typeof(VacancieLogic<>));
builder.Services.AddScoped(typeof(IResponseLogic<>), typeof(ResponseLogic<>));




builder.Services.AddSingleton<IConnection>(factory =>
{
    var rabbitMqFactory = new ConnectionFactory() { HostName = "localhost" };
    return rabbitMqFactory.CreateConnection();
});

builder.Services.AddSingleton<IModel>(provider =>
{
    var connection = provider.GetRequiredService<IConnection>();
    return connection.CreateModel();
});

builder.Services.AddScoped<IVacancieProducercs, VacancieProducer>();
builder.Services.AddScoped<IResponseProducer, ResponseProducer>();

builder.Services.AddSingleton<QueueListenerService>(provider =>
{
    var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
    using (var scope = scopeFactory.CreateScope())
    {
        var scopedProvider = scope.ServiceProvider;
        var producer = scopedProvider.GetRequiredService<IVacancieProducercs>();

        var listenerService = new QueueListenerService(producer);
        return listenerService;
    }
});

builder.Services.AddSingleton<QueueListenerResponse>(provider =>
{
    var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
    using (var scope = scopeFactory.CreateScope())
    {
        var scopedProvider = scope.ServiceProvider;
        var producer = scopedProvider.GetRequiredService<IResponseProducer>();

        var listenerService = new QueueListenerResponse(producer);
        return listenerService;
    }
});

builder.Services.AddSingleton<IHostedService>(provider =>
{
    var listenerService = provider.GetRequiredService<QueueListenerService>();
    return listenerService;
});
builder.Services.AddSingleton<IHostedService>(provider =>
{
    var listenerService = provider.GetRequiredService<QueueListenerResponse>();
    return listenerService;
});


builder.Services.AddScoped<LocationRpc>();
builder.Services.AddScoped<CompanyRpc>();
builder.Services.AddScoped<CvRpc>();


builder.Services.AddScoped<IVacancieService, VacancyServices>();
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

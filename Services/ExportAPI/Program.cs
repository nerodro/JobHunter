using ExportAPI.RabbitMq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using UserAPI.RabbitMq;
using UserRepository.CvLogic;
using UserRepository.LanguageLogic;
using UserRepository.UserContext;
using UserRepository.UserLogic;
using VacancieAPI.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<UserDbContext>(options => options.UseNpgsql(connection));

builder.Services.AddScoped(typeof(IUserLogic<>), typeof(UserLogic<>));
builder.Services.AddScoped(typeof(ICvLogic<>), typeof(CvLogic<>));
builder.Services.AddScoped(typeof(ILanguageLogic<>), typeof(LanguageLogic<>));

builder.Services.Configure<RabbitMQOptions>(builder.Configuration.GetSection("RabbitMQ"));

builder.Services.AddSingleton<IConnection>(provider =>
{
    var options = provider.GetRequiredService<IOptions<RabbitMQOptions>>().Value;
    var rabbitMqFactory = new ConnectionFactory
    {
        HostName = options.Hostname,
        UserName = options.Username,
        Password = options.Password
    };
    return rabbitMqFactory.CreateConnection();
});

builder.Services.AddSingleton<IModel>(provider =>
{
    var connection = provider.GetRequiredService<IConnection>();
    return connection.CreateModel();
});

builder.Services.AddSingleton<QueueListenerService>(provider =>
{
    var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
    using (var scope = scopeFactory.CreateScope())
    {
        var scopedProvider = scope.ServiceProvider;
        var producer = scopedProvider.GetRequiredService<IExportCv>();

        var listenerService = new QueueListenerService(producer);
        return listenerService;
    }
});

builder.Services.AddSingleton<IHostedService>(provider =>
{
    var listenerService = provider.GetRequiredService<QueueListenerService>();
    return listenerService;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("POLICY", builders =>
    {
        builders
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("POLICY");
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

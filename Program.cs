using Microsoft.Extensions.Options;
using WeatherAPIWrapperService.Configurations;
using WeatherAPIWrapperService.Serivces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.Configure<OpenWeatherMapSettings>(

    builder.Configuration.GetSection(nameof(OpenWeatherMapSettings))
);

builder.Services.Configure<CacheConfig>(
    builder.Configuration.GetSection(nameof(CacheConfig))
);


builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisCache");
    options.InstanceName = "WeatherApi_";
});

builder.Services.AddScoped<IWeatherService, WeatherService>();

builder.Services.AddHttpClient<WeatherService>((serviceProvider, client) =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<OpenWeatherMapSettings>>().Value;


    client.BaseAddress = new Uri(settings.BaseUrl);

    client.Timeout = TimeSpan.FromSeconds(15);
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

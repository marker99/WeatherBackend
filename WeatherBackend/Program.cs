using Microsoft.Extensions.Options;
using WeatherBackend;
using WeatherBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder => builder
            .WithOrigins("http://localhost:5173") 
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Load appsettings.Development.json
builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);

// Bind WeatherApiSettings from appsettings.Development.json
builder.Services.Configure<WeatherApiSettings>(
    builder.Configuration.GetSection("WeatherApiSettings"));

// Configure HttpClient for WeatherService
builder.Services.AddHttpClient<WeatherService>();

// Register WeatherService
builder.Services.AddSingleton(sp =>
{
    var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient();
    var apiKey = sp.GetRequiredService<IOptions<WeatherApiSettings>>().Value.ApiKey;
    return new WeatherService(httpClient, apiKey);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.UseAuthorization();

app.MapControllers();

app.Run();

using TinyUrl.Cache;
using TinyUrl.Dal;
using TinyUrl.Models;
using TinyUrl.Services;
using TinyUrl.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ITinyUrlGeneretor, TinyUrlGenerator>();
builder.Services.AddSingleton<ITinyUrlValidator, TinyUrlValidator>();
builder.Services.AddSingleton<ITinyUrlService, TinyUrlService>();
builder.Services.AddSingleton<ITinyUrlDal, TinyUrlDal>();
builder.Services.AddSingleton<ITinyUrlMongoDBClient, TinyUrlMongoDBClient>();
builder.Services.AddSingleton<IUrlMemoryCache, UrlMemoryCache>();
builder.Services.Configure<UrlsDatabaseSettings>(
builder.Configuration.GetSection("UrlsDataBase"));
builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection("Cache"));

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

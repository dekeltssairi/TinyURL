using TinyUrl.Models;
using TinyUrl.Services;
using TinyUrl.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ITinyUrlGeneretor, TinyUrlGenerator>();
builder.Services.AddScoped<ITinyUrlValidator, TinyUrlValidator>();
builder.Services.AddScoped<ITinyUrlService, TinyUrlService>();
builder.Services.AddSingleton<IBooksService, BooksService>();
builder.Services.Configure<BookStoreDatabaseSettings>(
    builder.Configuration.GetSection("BookStoreDatabase"));
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

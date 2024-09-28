using MoodCat.App.Core.Application;
using MoodCat.App.Core.Infrastructure;
using MoodCat.App.Core.Infrastructure.Data.Extensions;
using MoodCat.App.Core.WebAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddApplicationLayerServices()
    .AddInfrastructureLayerServices(builder.Configuration)
    .AddApiLayerServices(builder.Configuration);

// builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseApiLayerServices();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.InitializeDatabaseAsync(app.Configuration);
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

await app.RunAsync();
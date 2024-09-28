using System.Reflection;
using Microsoft.OpenApi.Models;
using MoodCat.App.Core.Application;
using MoodCat.App.Core.Domain.Users;
using MoodCat.App.Core.Infrastructure;
using MoodCat.App.Core.Infrastructure.Data.Extensions;
using MoodCat.App.Core.WebAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddApplicationLayerServices()
    .AddInfrastructureLayerServices(builder.Configuration)
    .AddApiLayerServices(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    opts.EnableAnnotations();

    opts.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "MoodCat App (Backend)",
        Version = "v1",
        Description = "Backend for MoodCat Application",
        Contact = new OpenApiContact()
        {
            Name = "Cyprian Gburek",
            Email = "dcyprian.a.gburek@gmail.com",
            Url = new Uri("https://github.com/SirCypkowskyy")
        }
    });

    // opts.AddSecurityDefinition("bearer", new OpenApiSecurityScheme()
    // {                
    //     In = ParameterLocation.Header,
    //     Name = "Authorization",
    //     Type = SecuritySchemeType.ApiKey,
    //     Description = "Please enter into field the word 'Bearer' following by space and JWT"
    // });

    // opts.OperationFilter<SecurityRequirementsOperationFilter>();

    // Add XML comments to Swagger
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
});

var app = builder.Build();

app.UseApiLayerServices();

app.UseSwagger();
app.UseSwaggerUI();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.InitializeDatabaseAsync(app.Configuration);
    app.UseDeveloperExceptionPage();
}   

// Microsoft Identity 
app.MapGroup("/api/auth/")
    .MapIdentityApi<User>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

await app.RunAsync();
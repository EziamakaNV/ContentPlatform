using Carter;
using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newsletter.Api.Database;
using Newsletter.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(assembly));

builder.Services.AddCarter();

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddMassTransit(busConfigurator => {
    busConfigurator.SetKebabCaseEndpointNameFormatter();
    // article-created-event

    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(new Uri(builder.Configuration["MessageBroker:Host"]!), hostConfigurator =>
        {
            hostConfigurator.Username(builder.Configuration["MessageBroker:Username"]);
            hostConfigurator.Password(builder.Configuration["MessageBroker:Password"]);
        });

        configurator.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.ConfigureCustomExceptionMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

app.MapCarter();

app.UseHttpsRedirection();



app.Run();

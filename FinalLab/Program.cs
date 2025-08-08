using Application.Behaviors;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Application.Commands;
using Infrastructure.Persistance;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LabDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<CreateDriverValidator>();

builder.Services.Configure<Microsoft.AspNetCore.Mvc.MvcOptions>(options =>
{
    var oldProvider = options.ModelValidatorProviders
        .FirstOrDefault(x => x is FluentValidation.AspNetCore.FluentValidationModelValidatorProvider);
    if (oldProvider != null)
    {
        options.ModelValidatorProviders.Remove(oldProvider);
    }
});


builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateVehicleCommand).Assembly));

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
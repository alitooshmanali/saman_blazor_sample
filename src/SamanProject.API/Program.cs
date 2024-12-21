using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SamanProject.API;
using SamanProject.Application;
using SamanProject.Application.Behaviors;
using SamanProject.Infrastructure.Context;
using SamanProject.Infrastructure.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddLogging(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddApiVersioning(options => options.ReportApiVersions = true);
builder.Services.AddHttpCacheHeaders();
builder.Services.AddControllers(options => options.ReturnHttpNotAcceptable = true)
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var validationProblemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Type = "",
                Title = "",
                Status = StatusCodes.Status422UnprocessableEntity,
                Detail = "",
                Instance = context.HttpContext.Request.Path
            };

            validationProblemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);

            return new UnprocessableEntityObjectResult(validationProblemDetails);
        };
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });


builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(container =>
    {
        var infrastructureAssembly = typeof(SamanDbContext).Assembly;
        var applicationAssembly = typeof(IUnitOfWork).Assembly;

        var configuration = MediatRConfigurationBuilder
                .Create(applicationAssembly)
                .WithAllOpenGenericHandlerTypesRegistered()
                .Build();

        container.RegisterMediatR(configuration);

        container.RegisterAssemblyTypes(infrastructureAssembly)
                    .Where(t => t.Name.EndsWith("Repository"))
                    .AsImplementedInterfaces().InstancePerLifetimeScope();


        container.RegisterAssemblyTypes(typeof(IUnitOfWork).Assembly)
                .AssignableTo(typeof(IUnitOfWork))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

        container.RegisterGeneric(typeof(TransactionBehavior<,>)).As(typeof(IPipelineBehavior<,>));
    });

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Saman Rest API", Version = "v1" });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextPool<SamanDbContext>(i => { i.UseNpgsql(connectionString); });

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

var serviceProvider = builder.Services.BuildServiceProvider();
var createScope = serviceProvider.CreateScope();
var serviceProviderFactory = createScope.ServiceProvider.GetService<IServiceScopeFactory>();

var dbContext = createScope.ServiceProvider.GetService<SamanDbContext>();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Saman Rest API v1"));
}

dbContext.Database.Migrate();
var dbConnection = dbContext.Database.GetDbConnection();
dbConnection.Open();
((NpgsqlConnection)dbConnection).ReloadTypes();
dbConnection.Close();

//DatabaseSeeder.Seed(serviceProviderFactory).GetAwaiter().GetResult();

app.UseHttpCacheHeaders();
app.UseRouting();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
//app.UseAuthorization();

app.Run();


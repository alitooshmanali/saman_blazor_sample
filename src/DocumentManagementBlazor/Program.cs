using Autofac.Extensions.DependencyInjection;
using Autofac;
using DocumentManagementBlazor;
using DocumentManagementBlazor.Data;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using MediatR;
using SamanProject.Infrastructure.Context;
using SamanProject.Application;
using MediatR.Extensions.Autofac.DependencyInjection;
using SamanProject.Application.Behaviors;
using SamanProject.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
builder.Services.AddLogging(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddApiVersioning(options => options.ReportApiVersions = true);
builder.Services.AddHttpCacheHeaders();
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
builder.Services.AddScoped<DocumentService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextPool<SamanDbContext>(i => { i.UseNpgsql(connectionString); });


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

var serviceProvider = builder.Services.BuildServiceProvider();
var createScope = serviceProvider.CreateScope();
var serviceProviderFactory = createScope.ServiceProvider.GetService<IServiceScopeFactory>();
var dbContext = createScope.ServiceProvider.GetService<SamanDbContext>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

dbContext.Database.Migrate();
var dbConnection = dbContext.Database.GetDbConnection();
dbConnection.Open();
((NpgsqlConnection)dbConnection).ReloadTypes();
dbConnection.Close();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

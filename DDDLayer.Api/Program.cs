using AspNetCoreRateLimit;
using DDDLayer.Application.DTOs;
using DDDLayer.Application.IoC;
using DDDLayer.Application.JWT;
using DDDLayer.Application.Utilities;
using DDDLayer.Domain.Entities;
using DDDLayer.Infrastructure.Context;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddFluentValidation(opt => opt.RegisterValidatorsFromAssemblyContaining<CreateUserDto>());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json",
                   optional: false,
                   reloadOnChange: true)
               .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                   optional: true,
                   reloadOnChange: true)
               
               .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .WriteTo.MSSqlServer(
        connectionString: configuration.GetConnectionString("DefaultConnection"), tableName: "Logs"
    )
    .CreateLogger();


builder.Services.AddSwaggerGen(builder =>
{
    builder.SwaggerDoc("ProjectV1", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Title="Project Api",
        Description="Project Operation",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
        {
            Email="ysn@gmail.com",
            Name="Yasin Kalender",
            Url= new System.Uri("https://github.com/YasinKalender")


        }
        
        
    
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    builder.IncludeXmlComments(xmlPath);

});



builder.Services.AddDbContext<ProjectContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOptions"));
builder.Services.Configure<Client>(builder.Configuration.GetSection("Client"));

builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ProjectContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
{
    var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<CustomTokenOption>();
    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidIssuer = tokenOptions.Issuer,
        ValidAudience = tokenOptions.Audience[0],
        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey),
        ValidateIssuerSigningKey = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        //ClockSkew = TimeSpan.Zero
    };


});

builder.Services.AddOptions();
builder.Services.AddMemoryCache();

builder.Services.Configure<ClientRateLimitOptions>(builder.Configuration.GetSection("ClientRateLimit"));
builder.Services.Configure<ClientRateLimitPolicies>(builder.Configuration.GetSection("ClientRateLimitPolicy"));
builder.Services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();


builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimit"));
builder.Services.Configure<IParameterPolicy>(builder.Configuration.GetSection("IpRateLimitPolicy"));
builder.Services.AddInMemoryRateLimiting();

builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IRateLimitConfiguration,RateLimitConfiguration>();




builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options=>options.SwaggerEndpoint("/swagger/ProjectV1/swagger.json", "Project API"));
}

app.Services.GetRequiredService<IIpPolicyStore>().SeedAsync().Wait();



app.UseIpRateLimiting();

app.UseClientRateLimiting();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using Pima.Api.Data;
using Pima.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register SecretClient for Azure Key Vault
var keyVaultUri = builder.Configuration["KeyVault:VaultUri"];
if (!string.IsNullOrEmpty(keyVaultUri))
{
    builder.Services.AddSingleton(new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential()));
}

// Register EF Core with Azure SQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlDatabase")));
builder.Services.AddScoped<IDatabaseHealthRepository, DatabaseHealthRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

var app = builder.Build();

// Enable Swagger in all environments so we can test from Azure App Service too
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

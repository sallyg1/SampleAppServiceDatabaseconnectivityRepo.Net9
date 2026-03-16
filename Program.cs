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
SecretClient? secretClient = null;
if (!string.IsNullOrEmpty(keyVaultUri))
{
    var credential = new DefaultAzureCredential();
    secretClient = new SecretClient(new Uri(keyVaultUri), credential);
    builder.Services.AddSingleton(secretClient);
}

// Retrieve SQL connection string from Key Vault, falling back to appsettings
string? sqlConnectionString = null;
if (secretClient is not null)
{
    var secret = await secretClient.GetSecretAsync("SqlDatabase");
    sqlConnectionString = secret.Value.Value;
}
sqlConnectionString ??= builder.Configuration.GetConnectionString("SqlDatabase");

// Register EF Core with Azure SQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(sqlConnectionString));
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

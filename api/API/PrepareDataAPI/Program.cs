using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrepareDataAPI.DBService;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsProduction())
{
    //var keyVault = $"https://{configuration["KeyVaultName"]}.vault.azure.net/";
    //var keyVaultClient = new KeyVaultClient(async (authority, resource, scope) =>
    //{
    // var credential = new DefaultAzureCredential(false);
    // var token = credential.GetToken(
    //    new Azure.Core.TokenRequestContext(
    //        new[] { "https://vault.azure.net/.default" }));
    // return token.Token;
    //});
    //configuration.AddAzureKeyVault(keyVault, keyVaultClient, new DefaultKeyVaultSecretManager());
    //configuration.AddAzureKeyVault(
    //    new Uri($"https://{configuration["KeyVaultName"]}.vault.azure.net/"),
    //    new DefaultAzureCredential());
    //new DefaultAzureCredential(new DefaultAzureCredentialOptions
    //{
    //    ManagedIdentityClientId = builder.Configuration["AzureADManagedIdentityClientId"]
    //})
    //);
    //builder.Services.AddDbContext<mydbContext>(options => options.UseMySQL(configuration.GetConnectionString("ConnectionStrings:MySQLRemoteConection")));
    //
    builder.Services.AddTransient<myDBConnect>(_ => new myDBConnect(configuration.GetConnectionString("MySQLRemoteConection")));
    builder.Services.AddTransient<cassandraDBConnect>(_ => new cassandraDBConnect(configuration.GetConnectionString("RemoteCassandra"), configuration.GetConnectionString("CassandraUser"), configuration.GetConnectionString("CassandraPass")));
}
else
    builder.Services.AddTransient<myDBConnect>(_ => new myDBConnect(configuration.GetConnectionString("MySQLLocalConnection")));
    builder.Services.AddTransient<cassandraDBConnect>(_ => new cassandraDBConnect(configuration.GetConnectionString("LocalCassandra"), configuration.GetConnectionString("CassandraUser"), configuration.GetConnectionString("CassandraPass")));

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

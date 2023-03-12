using Microsoft.EntityFrameworkCore;
using LeveldetailsAPI.DBService;
using Azure.Identity;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Net.Http.Headers;

internal class Program
{
    private static void Main(string[] args)
    {
        var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins,
                              policy =>
                              {
                                  policy.WithOrigins("http://localhost:3000", "https://white-field-09fbc4a03.2.azurestaticapps.net")
                                  .WithHeaders(HeaderNames.ContentType, "application/json")
                                  .WithHeaders(HeaderNames.Accept, "application/json")
                                  .WithMethods("POST", "PUT", "DELETE", "GET");
                              });
        });
        // Add services to the container.
        ConfigurationManager configuration = builder.Configuration;
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        if (builder.Environment.IsProduction())
        {
            //var keyVault = $"https://{configuration["KeyVaultName"]}.vault.azure.net/";
            //var keyVaultClient = new KeyVaultClient(async (authority, resource, scope) =>
            //{
            //    var credential = new DefaultAzureCredential(false);
            //    var token = credential.GetToken(
            //       new Azure.Core.TokenRequestContext(
            //           new[] { "https://vault.azure.net/.default" }));
            //    return token.Token;
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
            builder.Services.AddDbContext<mydbContext>(options => options.UseMySQL(configuration.GetConnectionString("MySQLRemoteConection")));
        }
        else
            builder.Services.AddDbContext<mydbContext>(options => options.UseMySQL(configuration.GetConnectionString("MySQLLocalConnection")));
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors(MyAllowSpecificOrigins);

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
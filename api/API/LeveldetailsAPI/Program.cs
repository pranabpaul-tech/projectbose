using Microsoft.EntityFrameworkCore;
using LeveldetailsAPI.DBService;
using Azure.Identity;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        ConfigurationManager configuration = builder.Configuration;
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        if (builder.Environment.IsProduction())
        {
            configuration.AddAzureKeyVault(
                new Uri($"https://{configuration["KeyVaultName"]}.vault.azure.net/"),
                new DefaultAzureCredential(new DefaultAzureCredentialOptions
                {
                    ManagedIdentityClientId = builder.Configuration["AzureADManagedIdentityClientId"]
                }));
            builder.Services.AddDbContext<mydbContext>(options => options.UseMySQL(configuration.GetConnectionString("ConnectionStrings:MySQLRemoteConection")));
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

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
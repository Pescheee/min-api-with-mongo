using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

var movieDatabaseConfigSection = builder.Configuration.GetSection("DatabaseSettings");
builder.Services.Configure<DatabaseSettings>(movieDatabaseConfigSection);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/check", (Microsoft.Extensions.Options.IOptions<DatabaseSettings> options) =>
{
    var mongoDbConnectionString = options.Value.ConnectionString;
 
    var error = "Fehler beim Zugriff auf MongoDB";
    try
    {
        var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        var cancellationToken = cancellationTokenSource.Token;
 
        var client = new MongoClient(mongoDbConnectionString);
 
        var databases = client.ListDatabaseNames(cancellationToken).ToList();
 
        return Results.Ok("Zugriff auf MongoDB ok. Datenbanken: " + string.Join(", ", databases));
    }
    catch (TimeoutException ex)
    {
        return Results.Problem(error + " (timout): " + ex.Message);
    }
    catch (Exception ex)
    {
        return Results.Problem(error + " :" + ex.Message);
    }
});

app.Run();

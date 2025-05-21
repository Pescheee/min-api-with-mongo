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

// Insert Movie
// Wenn das übergebene Objekt eingefügt werden konnte,
// wird es mit Statuscode 200 zurückgegeben.
// Bei Fehler wird Statuscode 409 Conflict zurückgegeben.
app.MapPost("/api/movies", (Movie movie) =>
{
return Results.Ok("Movie has been created");
});
// Get all Movies
// Gibt alle vorhandenen Movie-Objekte mit Statuscode 200 OK zurück.
app.MapGet("api/movies", () =>
{
return Results.Ok("Movie: XYZ, Year: 2001, etc.");
});
// Get Movie by id
// Gibt das gewünschte Movie-Objekt mit Statuscode 200 OK zurück.
// Bei ungültiger id wird Statuscode 404 not found zurückgegeben.
app.MapGet("api/movies/{id}", (string id) =>
{
return Results.Ok("Movie: XYZ, Year: 2001, etc.");
});
// Update Movie
// Gibt das aktualisierte Movie-Objekt zurück.
// Bei ungültiger id wird Statuscode 404 not found zurückgegeben.
app.MapPut("/api/movies/{id}", (string id, Movie movie) =>
{
return Results.Ok("Movie has been Updated");
});
// Delete Movie
// Gibt bei erfolgreicher Löschung Statuscode 200 OK zurück.
// Bei ungültiger id wird Statuscode 404 not found zurückgegeben.
app.MapDelete("api/movies/{id}", (string id) =>
{
return Results.Ok("Movie has been deleted Succesfully");
});

app.Run();

using Microsoft.Extensions.Options;

using MongoDB.Driver;

public class MongoMovieService : IMovieService

{

    // Constructor.

    // Settings werden per dependency injection übergeben.

    private readonly MongoClient client;

    private readonly IMongoCollection<Movie> collection;
 
    public MongoMovieService(IOptions<DatabaseSettings> options)

    {

        client = new MongoClient(options.Value.ConnectionString);

        var database = client.GetDatabase("gbs");

        collection = database.GetCollection<Movie>("movies");

    }
 
    public string Check()

    {

        try

        {

            var dbs = client.ListDatabaseNames().ToList();

            return $"Zugriff auf MongoDB ok. Vorhandene DBs: {string.Join(",", dbs)}";

        }

        catch (Exception ex)

        {

            return $"Fehler beim Zugriff auf MongoDB: {ex.Message}";

        }

    }
 
    public void Create(Movie movie)

    {

        collection.InsertOne(movie);

    }
 
    public IEnumerable<Movie> Get()

    {

        return collection.Find(_ => true).ToList();

    }
 
    public Movie Get(string id)

    {

        return collection.Find(m => m.Id == id).ToList().First();

    }
 
    public void Remove(string id)

    {

        collection.DeleteOne(m => m.Id == id);

    }
 
    public void Update(string id, Movie movie)

    {

        collection.ReplaceOne(m => m.Id == id, movie);

    }

}
 
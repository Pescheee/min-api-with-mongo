using MongoDB.Bson.Serialization.Attributes;
 
public class Movie {
    [BsonId]
    public string Id {get; set;}
    public string Title {get; set;} = "";
    public int Year {get; set;}
    public string summary {get; set;} = "";
    public string[] Actors {get; set;} = Array.Empty<string>();
 
    public Movie() {}
    public Movie(string i, string t, int y, string s, string[] a) {
        this.Id = i;
        this.Title = t;
        this.Year = y;
        this.summary = s;
        this.Actors = a;
    }
}
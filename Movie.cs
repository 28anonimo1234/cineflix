using System;
using System.Xml.Serialization;

[Serializable]
public class Movie
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public int Year { get; set; }
    public double Rating { get; set; }
    public string PosterPath { get; set; }

    public Movie()
    {
        Id = Guid.NewGuid().ToString();
    }
}

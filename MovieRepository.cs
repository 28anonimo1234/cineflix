using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

public class MovieRepository
{
    private readonly string filePath;
    public List<Movie> Movies { get; private set; } = new List<Movie>();

    public MovieRepository(string filePath)
    {
        this.filePath = filePath;
        Load();
    }

    public void Load()
    {
        try
        {
            if (!File.Exists(filePath))
            {
                Movies = new List<Movie>();
                return;
            }
            var serializer = new XmlSerializer(typeof(List<Movie>));
            using (var fs = File.OpenRead(filePath))
            {
                Movies = (List<Movie>)serializer.Deserialize(fs);
            }
        }
        catch
        {
            Movies = new List<Movie>();
        }
    }

    public void Save()
    {
        var serializer = new XmlSerializer(typeof(List<Movie>));
        using (var fs = File.Create(filePath))
        {
            serializer.Serialize(fs, Movies);
        }
    }

    public void Add(Movie m)
    {
        Movies.Add(m);
        Save();
    }

    public void Update(Movie m)
    {
        var idx = Movies.FindIndex(x => x.Id == m.Id);
        if (idx >= 0) Movies[idx] = m;
        Save();
    }

    public void Delete(string id)
    {
        Movies.RemoveAll(x => x.Id == id);
        Save();
    }
}

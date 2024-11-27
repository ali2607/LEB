using LEB_API.Models;
using System.Text.Json;

public static class JsonHelper
{
    private static readonly string FilePath = "notes.json";

    public static List<Notes> ReadNotes()
    {
        if (!File.Exists(FilePath))
        {
            return new List<Notes>();
        }
        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<Notes>>(json) ?? new List<Notes>();
    }

    public static void WriteNotes(List<Notes> notes)
    {
        var json = JsonSerializer.Serialize(notes);
        File.WriteAllText(FilePath, json);
    }

}

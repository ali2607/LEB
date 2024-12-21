using LEB_API.Models;
using System.Text.Json;

public static class JsonHelper
{
    private static readonly string FilePath = "notes.json";

    public static List<Note> ReadNotes()
    {
        if (!File.Exists(FilePath))
        {
            return [];
        }
        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<Note>>(json) ?? [];
    }

    public static void WriteNotes(List<Note> notes)
    {
        var json = JsonSerializer.Serialize(notes);
        File.WriteAllText(FilePath, json);
    }

}

using LEB_API.Models;
using System.Text.Json;

namespace LEB_API.Services;

/// <summary>
/// Provides helper methods for reading and writing <see cref="Note"/> objects to and from a JSON file.
/// </summary>
public static class JsonHelper
{
    /// <summary>
    /// The file path where notes are stored.
    /// </summary>
    private static readonly string FilePath = "notes.json";

    /// <summary>
    /// Reads the notes from the JSON file. If the file does not exist or is empty, an empty list is returned.
    /// </summary>
    /// <returns>A list of <see cref="Note"/> objects.</returns>
    public static List<Note> ReadNotes()
    {
        if (!File.Exists(FilePath))
        {
            return [];
        }
        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<Note>>(json) ?? [];
    }

    /// <summary>
    /// Writes the given list of notes to the JSON file, overwriting any existing content.
    /// </summary>
    /// <param name="notes">The list of notes to write.</param>
    public static void WriteNotes(List<Note> notes)
    {
        var json = JsonSerializer.Serialize(notes);
        File.WriteAllText(FilePath, json);
    }

}

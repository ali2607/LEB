using System.Text.Json;
using System.IO;
using LEB_API.Models;
using System;

public static class JsonHelper
{
    private static readonly string FilePath = GetFilePath(); // Adjust path using environment variable

    public static List<Notes> ReadNotes()
    {
        try
        {
            if (!File.Exists(FilePath))
            {
                // Initialize a new file if it doesn't exist
                WriteNotes(new List<Notes>());
                return new List<Notes>();
            }

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Notes>>(json) ?? new List<Notes>();
        }
        catch (JsonException ex)
        {
            // Handle corrupted JSON file
            Console.WriteLine($"JSON is corrupted: {ex.Message}");
            WriteNotes(new List<Notes>()); // Overwrite with an empty list
            return new List<Notes>();
        }
    }

    public static void WriteNotes(List<Notes> notes)
    {
        try
        {
            // Ensure the directory exists
            var directory = Path.GetDirectoryName(FilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory); // Create the directory if it doesn't exist
            }

            // Ensure the file exists and write to it
            var json = JsonSerializer.Serialize(notes, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
        catch (Exception ex)
        {
            // Handle potential file write errors
            Console.WriteLine($"Failed to write to JSON file: {ex.Message}");
        }
    }

    // Method to get the file path from environment variables or default to a relative path
    private static string GetFilePath()
    {
        var customFilePath = Environment.GetEnvironmentVariable("NOTES_FILE_PATH");
        return string.IsNullOrEmpty(customFilePath) ? "Data/notes.json" : customFilePath;
    }
}

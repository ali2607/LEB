using LEB_API.Models;
using LEB_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace LEB_API.Controllers;

/// <summary>
/// Controller responsible for managing notes.
/// Provides endpoints to create, retrieve, update, and delete notes.
/// </summary>
[ApiController]
[Route("api/notes")]
public class NotesController : ControllerBase
{
    /// <summary>
    /// Retrieves all notes.
    /// </summary>
    /// <returns>A list of all notes.</returns>
    [HttpGet]
    public IActionResult GetAll()
    {
        var notes = JsonHelper.ReadNotes();
        return Ok(notes);
    }

    /// <summary>
    /// Retrieves a specific note by its ID.
    /// </summary>
    /// <param name="id">The ID of the note to retrieve.</param>
    /// <returns>The note with the specified ID.</returns>
    [HttpGet("{id:long}")]
    public IActionResult GetById(long id)
    {
        var notes = JsonHelper.ReadNotes();
        var note = notes.FirstOrDefault(n => n.Id == id);
        return note == null ? NotFound() : Ok(note);
    }

    /// <summary>
    /// Creates a new note.
    /// </summary>
    /// <param name="newNote">The new note to create.</param>
    /// <returns>The created note with its assigned ID.</returns>
    [HttpPost]
    public IActionResult Create([FromBody] Note newNote)
    {
        if (string.IsNullOrWhiteSpace(newNote.Content))
        {
            return BadRequest("Content cannot be empty.");
        }

        var notes = JsonHelper.ReadNotes();

        // Assign the next available ID and set CreatedDate
        newNote.Id = notes.Count > 0 ? notes.Max(n => n.Id) + 1 : 1;
        newNote.CreatedDate = DateTime.Now;

        notes.Add(newNote);
        JsonHelper.WriteNotes(notes);

        return CreatedAtAction(nameof(GetById), new { id = newNote.Id }, newNote);
    }

    /// <summary>
    /// Updates an existing note.
    /// </summary>
    /// <param name="id">The ID of the note to update.</param>
    /// <param name="updatedNote">The updated note data.</param>
    [HttpPut("{id:long}")]
    public IActionResult Update(long id, [FromBody] Note updatedNote)
    {
        var notes = JsonHelper.ReadNotes();
        var existingNote = notes.FirstOrDefault(n => n.Id == id);

        if (existingNote == null)
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(updatedNote.Content))
        {
            return BadRequest("Content cannot be empty.");
        }

        // Update existing note fields and set UpdatedDate
        existingNote.Content = updatedNote.Content;
        existingNote.Summary = updatedNote.Summary;
        existingNote.SummarizedDate = DateTime.Now;
        existingNote.UpdatedDate = DateTime.Now;

        JsonHelper.WriteNotes(notes);
        return NoContent();
    }

    /// <summary>
    /// Deletes a specific note by its ID.
    /// </summary>
    /// <param name="id">The ID of the note to delete.</param>
    [HttpDelete("{id:long}")]
    public IActionResult Delete(long id)
    {
        var notes = JsonHelper.ReadNotes();
        var note = notes.FirstOrDefault(n => n.Id == id);

        if (note == null)
        {
            return NotFound();
        }

        notes.Remove(note);
        JsonHelper.WriteNotes(notes);
        return NoContent();
    }
}

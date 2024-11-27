using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("api/notes")]
public class NotesController : ControllerBase
{
    private static readonly List<Note> Notes = new();

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(Notes);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var note = Notes.FirstOrDefault(n => n.Id == id);
        return note == null ? NotFound() : Ok(note);
    }

    [HttpPost]
    public IActionResult Create([FromBody] Note newNote)
    {
        if (string.IsNullOrWhiteSpace(newNote.Title) || string.IsNullOrWhiteSpace(newNote.Content))
        {
            return BadRequest("Title and content cannot be empty.");
        }

        newNote.Id = Notes.Count > 0 ? Notes.Max(n => n.Id) + 1 : 1;
        Notes.Add(newNote);
        return CreatedAtAction(nameof(GetById), new { id = newNote.Id }, newNote);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] Note updatedNote)
    {
        var existingNote = Notes.FirstOrDefault(n => n.Id == id);
        if (existingNote == null)
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(updatedNote.Title) || string.IsNullOrWhiteSpace(updatedNote.Content))
        {
            return BadRequest("Title and content cannot be empty.");
        }

        existingNote.Title = updatedNote.Title;
        existingNote.Content = updatedNote.Content;
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var note = Notes.FirstOrDefault(n => n.Id == id);
        if (note == null)
        {
            return NotFound();
        }

        Notes.Remove(note);
        return NoContent();
    }

    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}

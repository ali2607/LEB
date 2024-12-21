using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using LEB_API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

[ApiController]
[Route("api/notes")]
public class NotesController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        var notes = JsonHelper.ReadNotes();
        return Ok(notes);
    }

    [HttpGet("{id:long}")]
    public IActionResult GetById(long id)
    {
        var notes = JsonHelper.ReadNotes();
        var note = notes.FirstOrDefault(n => n.Id == id);
        return note == null ? NotFound() : Ok(note);
    }

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

}

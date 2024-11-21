using Microsoft.AspNetCore.Mvc;
using LEB_API.Models;
using System.Linq;

namespace LEB_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Notes newNote)
        {
            var notes = JsonHelper.ReadNotes();
            newNote.Id = notes.Count > 0 ? notes.Max(n => n.Id) + 1 : 1;
            newNote.CreatedDate = DateTime.Now;
            newNote.UpdatedDate = DateTime.Now;
            notes.Add(newNote);
            JsonHelper.WriteNotes(notes);
            return CreatedAtAction(nameof(GetById), new { id = newNote.Id }, newNote);
        }

        [HttpPut("{id:long}")]
        public IActionResult Update(long id, [FromBody] Notes updatedNote)
        {
            var notes = JsonHelper.ReadNotes();
            var note = notes.FirstOrDefault(n => n.Id == id);
            if (note == null)
            {
                return NotFound();
            }
            note.Title = updatedNote.Title;
            note.Summary = updatedNote.Summary;
            note.UpdatedDate = DateTime.Now;
            JsonHelper.WriteNotes(notes);
            return NoContent();
        }

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
}

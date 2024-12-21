namespace LEB_API.Models;

/// <summary>
/// Represents a request to manipulate notes based on a user's query.
/// </summary>
public class ManipulationRequest
{
    /// <summary>
    /// Gets or sets the user's query or request that will be processed by the LLM.
    /// </summary>
    public string Request { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the collection of notes that the LLM will consider when processing the request.
    /// </summary>
    public List<Note> Notes { get; set; } = [];
}

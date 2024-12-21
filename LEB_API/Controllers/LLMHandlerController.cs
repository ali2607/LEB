using LEB_API.Models;
using Microsoft.AspNetCore.Mvc;
using OpenAI.Chat;
using System.Text.Json;

namespace LEB_API.Controllers;

/// <summary>
/// Controller for handling LLM-based operations.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class LLMHandlerController : ControllerBase
{
    /// <summary>
    /// The API key used for authentication with the LLM API.
    /// </summary>
    private readonly string _apiKey;

    /// <summary>
    /// The ChatClient instance used to interact with the GPT LLM.
    /// </summary>
    private readonly ChatClient _chatClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="LLMHandlerController"/> class.
    /// </summary>
    /// <param name="configuration">The application configuration (containing the API key).</param>
    /// <exception cref="ArgumentNullException">Thrown when the API key is missing in the configuration.</exception>
    public LLMHandlerController(IConfiguration configuration)
    {
        _apiKey = configuration["ApiKeys:MyApiKey"] ?? throw new ArgumentNullException("API key is missing");
        _chatClient = new("gpt-4o", _apiKey);
    }

    /// <summary>
    /// Summarizes the content of a note.
    /// </summary>
    /// <param name="noteContent">The content of the note to summarize.</param>
    /// <returns>A summary of the note.</returns>
    [HttpGet("summary")]
    public async Task<IActionResult> SummarizeNoteAsync([FromQuery] string noteContent)
    {
        if (string.IsNullOrWhiteSpace(noteContent))
            return BadRequest(new { error = "Note content cannot be empty." });

        var prompt = $@"
        You are provided with a note in HTML format. Your task is to summarize the content of the note in a concise and accurate manner.

        **Important Instructions:**
        1. **HTML Output Only:** Return only the summary in valid HTML format. Do not include code fences, additional explanations, or formatting outside of HTML tags.
        2. **Conciseness:** The summary must be brief. Avoid repeating the original content verbatim.
        3. **Language Matching:** Your summary must be in the same language as the input note.
        4. **Error Handling:** If the note content is unclear or incomplete, indicate this in the summary, using a simple message like: `<p>The note content is unclear or incomplete.</p>`

        **Note Content:**
        {noteContent}
        ";

        try
        {
            var message = new UserChatMessage(prompt);
            ChatCompletion response = await _chatClient.CompleteChatAsync(message);
            return Content(response.Content[0].Text, "text/html");
        }
        catch (Exception e)
        {
            return StatusCode(500, new { error = e.Message });
        }
    }

    /// <summary>
    /// Manipulates notes based on a user request.
    /// </summary>
    /// <param name="manipulationRequest">The manipulation request, containing the request and the list of notes.</param>
    /// <returns>The manipulated list of notes or relevant information.</returns>
    [HttpPost("manipulation-query")]
    public async Task<IActionResult> ManipulateNotesAsync([FromBody] ManipulationRequest manipulationRequest)
    {
        if (manipulationRequest == null || string.IsNullOrWhiteSpace(manipulationRequest.Request) || manipulationRequest.Notes == null)
            return BadRequest(new { error = "Invalid request or notes data." });

        string notesJson = JsonSerializer.Serialize(manipulationRequest.Notes);
        var prompt = $@"
        You are provided with a user's request and a list of notes in JSON format. 
        Your task is to analyze the request and either modify the notes or provide information based on the user's instructions.

        **Important Guidelines:**
        1. **Structured Response:** Your response must adhere to the provided JSON schema strictly.
           - Deleting notes is **not allowed** under any circumstances. If the request involves deleting notes, respond with a message indicating that note deletion is not permitted.
        2. **Consistency and Validity:** Ensure that the JSON format in your response is valid and corresponds to the notes schema provided in the input.
        3. **Preserve HTML Format:** Your response must be properly formatted HTML and should NOT use Markdown or other formats like `**` for bold or `_` for italics.For example, use `<b>` for bold and `<i>` for italics etc...
        4. **Relevance to Notes and Related Context:** Primarily respond to questions or requests directly related to the provided notes. However, if the user's request has a close or indirect connection to the content of the notes, provide a response that considers these relationships or contextual links. 
           - If no relevant or related information can reasonably be inferred from the notes, respond with a message indicating that the notes do not contain information sufficiently related to the user's request.
           - Be proactive in identifying links or contextual interpretations while remaining faithful to the content of the notes.
        5. **Language Matching:** Use the same language as the user's request when crafting responses or generating content.
        6. **Date Awareness:** Ensure that date-related information in the notes is properly handled and preserved in its original format, unless modification is explicitly required.
        7. **Error Handling:** If the request is unclear or invalid, provide a meaningful error message in the """"content"""" field and set the """"type"""" field to """"information.""""
        9. **Unsupported Operations:** If the request includes unsupported operations (e.g., file uploads, external searches), respond with a message indicating that the operation is not supported.
        10. **Input Validation:** Always validate and sanitize user inputs to ensure they are free from potentially harmful content (e.g., malicious code, unsupported formats).
        11. **Schema Adherence:** Ensure the modified notes strictly adhere to the original schema. Any new notes must also follow the same schema.
        12. **Ambiguity Handling:** If the user's request is unclear, precise it in the question to show the user that you didn't fully understand the prompt, and try to give the most appropriate answer if possible.
        13. **Note Relations:** Remember that there could be relations between notes. Before answering to my """"information"""" request, check if the information is related to other notes.

        **User Request:** {manipulationRequest.Request}
        **Notes JSON:** {notesJson}

        ";

        ChatCompletionOptions options = new()
        {
            ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
            "NotesManipulation",
            BinaryData.FromBytes("""
            {
                "type": "object",
                "properties": {
                    "type": {
                        "type": "string",
                        "enum": ["modification", "information"]
                    },
                    "content": {
                        "type": "string"
                    }
                },
                "required": ["type", "content"],
                "additionalProperties": false
            }

            """u8.ToArray()),
            "If the request is about modifying or adding a note, then response type is 'modification' and the 'content' field should contain the modified notes in JSON format. If the request is about information retrieval, then response type is 'information' and the 'content' field should contain a natural language response.",
            true
            )
        };

        try
        {
            var message = new UserChatMessage(prompt);
            ChatCompletion response = await _chatClient.CompleteChatAsync([message], options);
            return Ok(new { result = response.Content[0].Text });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { error = e.Message });
        }
    }
}
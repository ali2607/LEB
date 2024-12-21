namespace LEB_API.Models
{
    public class Note
    {
        public long Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? Summary { get; set; } = string.Empty;
        public DateTime? CreatedDate { get; set; } = default;
        public DateTime? UpdatedDate { get; set; } = default;
        public DateTime? SummarizedDate { get; set; } = default;
    }

}

namespace LEB_API.Models
{
    public class Notes
    {
        public long Id { get; set; }
        public string? Title { get; set; } = string.Empty; 
        public string Content { get; set; } = string.Empty; 
        public string? Summary { get; set; } = string.Empty; 
        public DateTime? CreatedDate { get; set; } = default;
        public DateTime? UpdatedDate { get; set; } = default;
        public DateTime? SummarizedDate { get; set; } = default;
    }
}

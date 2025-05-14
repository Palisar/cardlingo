namespace FlipCardApp.Domain
{
    public class Card
    {
        public int Id { get; set; }
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastModified { get; set; } = DateTime.UtcNow;
        
        // Foreign keys
        public int DeckId { get; set; }
        
        // Navigation properties
        public Deck Deck { get; set; } = null!;
        public ICollection<ReviewHistory> ReviewHistories { get; set; } = new List<ReviewHistory>();
    }
}

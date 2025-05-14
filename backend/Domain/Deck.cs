namespace FlipCardApp.Domain
{
    public class Deck
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastModified { get; set; } = DateTime.UtcNow;
        
        // Foreign keys
        public int UserId { get; set; }
        
        // Navigation properties
        public User User { get; set; } = null!;
        public ICollection<Card> Cards { get; set; } = new List<Card>();
        public ICollection<Pile> Piles { get; set; } = new List<Pile>();
    }
}

namespace FlipCardApp.Domain
{
    public class Pile
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Order { get; set; } // For representing spaced repetition levels
        
        // Foreign keys
        public int DeckId { get; set; }
        
        // Navigation properties
        public Deck Deck { get; set; } = null!;
        public ICollection<Card> Cards { get; set; } = new List<Card>();
    }
}

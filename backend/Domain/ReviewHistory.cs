namespace FlipCardApp.Domain
{
    public class ReviewHistory
    {
        public int Id { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.UtcNow;
        public int Score { get; set; } // A measure of how well the user remembered the card
        public TimeSpan ResponseTime { get; set; } // How long it took to respond
        
        // Foreign keys
        public int CardId { get; set; }
        public int UserId { get; set; }
        
        // Navigation properties
        public Card Card { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}

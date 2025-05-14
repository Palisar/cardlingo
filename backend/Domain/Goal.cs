namespace FlipCardApp.Domain
{
    public class Goal
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int TargetCount { get; set; } // Number of cards to review
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCompleted { get; set; }
        
        // Foreign keys
        public int UserId { get; set; }
        
        // Navigation properties
        public User User { get; set; } = null!;
    }
}

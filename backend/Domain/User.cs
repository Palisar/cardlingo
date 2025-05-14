namespace FlipCardApp.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastLogin { get; set; }

        // Navigation properties
        public ICollection<Deck> Decks { get; set; } = new List<Deck>();
        public ICollection<Goal> Goals { get; set; } = new List<Goal>();
    }
}

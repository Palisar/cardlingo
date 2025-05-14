using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FlipCardApp.Domain;
using FlipCardApp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FlipCardApp.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Card>> GetCard(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            
            var card = await _context.Cards
                .Include(c => c.Deck)
                .FirstOrDefaultAsync(c => c.Id == id && c.Deck.UserId == userId);

            if (card == null)
            {
                return NotFound();
            }

            return card;
        }

        [HttpPost]
        public async Task<ActionResult<Card>> CreateCard(Card card)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            
            // Verify the deck belongs to the user
            var deck = await _context.Decks.FirstOrDefaultAsync(d => d.Id == card.DeckId && d.UserId == userId);
            if (deck == null)
            {
                return BadRequest("Invalid deck");
            }
            
            card.Created = DateTime.UtcNow;
            card.LastModified = DateTime.UtcNow;
            
            _context.Cards.Add(card);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCard), new { id = card.Id }, card);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCard(int id, Card card)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            
            if (id != card.Id)
            {
                return BadRequest();
            }

            // Verify the card exists and belongs to the user
            var existingCard = await _context.Cards
                .Include(c => c.Deck)
                .FirstOrDefaultAsync(c => c.Id == id && c.Deck.UserId == userId);
                
            if (existingCard == null)
            {
                return NotFound();
            }

            existingCard.Question = card.Question;
            existingCard.Answer = card.Answer;
            existingCard.Notes = card.Notes;
            existingCard.LastModified = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CardExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCard(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            
            var card = await _context.Cards
                .Include(c => c.Deck)
                .FirstOrDefaultAsync(c => c.Id == id && c.Deck.UserId == userId);
                
            if (card == null)
            {
                return NotFound();
            }

            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        [HttpGet("deck/{deckId}")]
        public async Task<ActionResult<IEnumerable<Card>>> GetCardsByDeck(int deckId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            
            // Verify deck belongs to user
            var deck = await _context.Decks.FirstOrDefaultAsync(d => d.Id == deckId && d.UserId == userId);
            if (deck == null)
            {
                return NotFound("Deck not found");
            }
            
            var cards = await _context.Cards
                .Where(c => c.DeckId == deckId)
                .ToListAsync();
                
            return cards;
        }

        private async Task<bool> CardExists(int id)
        {
            return await _context.Cards.AnyAsync(e => e.Id == id);
        }
    }
}

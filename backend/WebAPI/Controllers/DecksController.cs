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
    public class DecksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DecksController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Deck>>> GetDecks()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            return await _context.Decks
                .Where(d => d.UserId == userId)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Deck>> GetDeck(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var deck = await _context.Decks
                .Include(d => d.Cards)
                .FirstOrDefaultAsync(d => d.Id == id && d.UserId == userId);

            if (deck == null)
            {
                return NotFound();
            }

            return deck;
        }

        [HttpPost]
        public async Task<ActionResult<Deck>> CreateDeck(Deck deck)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            
            deck.UserId = userId;
            deck.Created = DateTime.UtcNow;
            deck.LastModified = DateTime.UtcNow;
            
            _context.Decks.Add(deck);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDeck), new { id = deck.Id }, deck);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeck(int id, Deck deck)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            
            if (id != deck.Id)
            {
                return BadRequest();
            }

            var existingDeck = await _context.Decks
                .FirstOrDefaultAsync(d => d.Id == id && d.UserId == userId);
                
            if (existingDeck == null)
            {
                return NotFound();
            }

            existingDeck.Name = deck.Name;
            existingDeck.Description = deck.Description;
            existingDeck.LastModified = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await DeckExists(id))
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
        public async Task<IActionResult> DeleteDeck(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var deck = await _context.Decks
                .FirstOrDefaultAsync(d => d.Id == id && d.UserId == userId);
                
            if (deck == null)
            {
                return NotFound();
            }

            _context.Decks.Remove(deck);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> DeckExists(int id)
        {
            return await _context.Decks.AnyAsync(e => e.Id == id);
        }
    }
}

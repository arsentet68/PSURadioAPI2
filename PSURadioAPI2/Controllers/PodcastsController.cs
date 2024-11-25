using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PSURadioAPI2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PodcastsController : ControllerBase
    {
        private readonly PsuradioContext _context;

        public PodcastsController(PsuradioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PodcastDto>>> GetPodcasts()
        {
            var podcastsList = await _context.Podcasts
                                         .OrderByDescending(n => n.Date)
                                         .Select(n => new PodcastDto
                                         {
                                             Id = n.Id,
                                             Title = n.Title,
                                             Text = n.Text,
                                             Audio = n.Audio,
                                             Image = n.Image, // Массив байт
                                             PublishedDate = n.Date
                                         })
                                         .ToListAsync();
            return podcastsList;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePodcast(int id, [FromBody] PodcastDto podcastDto)
        {
            if (id != podcastDto.Id)
            {
                return BadRequest();
            }

            var podcast = await _context.Podcasts.FindAsync(id);
            if (podcast == null)
            {
                return NotFound();
            }

            podcast.Title = podcastDto.Title;
            podcast.Text = podcastDto.Text;
            podcast.Image = podcastDto.Image;
            podcast.Audio = podcastDto.Audio;
            podcast.Date = podcastDto.PublishedDate;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PodcastExists(id))
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
        [HttpPost]
        public async Task<ActionResult<PodcastDto>> PostPodcast([FromBody] PodcastDto podcastDto)
        {
            var podcast = new Podcast
            {
                Title = podcastDto.Title,
                Text = podcastDto.Text,
                Image = podcastDto.Image,
                Date = podcastDto.PublishedDate.HasValue ? podcastDto.PublishedDate.Value.ToUniversalTime() : DateTime.UtcNow
            };

            _context.Podcasts.Add(podcast);
            await _context.SaveChangesAsync();

            podcastDto.Id = podcast.Id;

            return CreatedAtAction(nameof(GetPodcasts), new { id = podcastDto.Id }, podcastDto);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePodcast(int id)
        {
            var podcast = await _context.Podcasts.FindAsync(id);
            if (podcast == null)
            {
                return NotFound();
            }

            _context.Podcasts.Remove(podcast);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool PodcastExists(int id)
        {
            return _context.Podcasts.Any(e => e.Id == id);
        }
        public class PodcastDto
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Text { get; set; }
            public byte[] Audio { get; set; }
            public byte[]? Image { get; set; }
            public DateTime? PublishedDate { get; set; }
        }
    }
}

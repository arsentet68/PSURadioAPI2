using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSURadioAPI2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly PsuradioContext _context;

        public NewsController(PsuradioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewsDto>>> GetNews()
        {
            var newsList = await _context.News
                                         .OrderByDescending(n => n.Date)
                                         .Select(n => new NewsDto
                                         {
                                             Id = n.Id,
                                             Title = n.Title,
                                             Text = n.Text,
                                             Image = n.Image, // Массив байт
                                             PublishedDate = n.Date
                                         })
                                         .ToListAsync();
            return newsList;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNews(int id, [FromBody] NewsDto newsDto)
        {
            if (id != newsDto.Id)
            {
                return BadRequest();
            }

            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }

            news.Title = newsDto.Title;
            news.Text = newsDto.Text;
            news.Image = newsDto.Image;
            news.Date = newsDto.PublishedDate;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsExists(id))
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
        public async Task<ActionResult<NewsDto>> PostNews([FromBody] NewsDto newsDto)
        {
            var news = new News
            {
                Title = newsDto.Title,
                Text = newsDto.Text,
                Image = newsDto.Image,
                Date = newsDto.PublishedDate.HasValue ? newsDto.PublishedDate.Value.ToUniversalTime() : DateTime.UtcNow
            };

            _context.News.Add(news);
            await _context.SaveChangesAsync();

            newsDto.Id = news.Id;

            return CreatedAtAction(nameof(GetNews), new { id = newsDto.Id }, newsDto);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }

            _context.News.Remove(news);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.Id == id);
        }
    }

    public class NewsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public byte[]? Image { get; set; }
        public DateTime? PublishedDate { get; set; }
    }
}

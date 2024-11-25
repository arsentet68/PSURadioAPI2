using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSURadioAPI2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistsController : ControllerBase
    {
        private readonly PsuradioContext _context;

        public PlaylistsController(PsuradioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaylistDto>>> GetPlaylists()
        {
            var playlistsList = await _context.Playlists
                                         .OrderByDescending(n => n.Date)
                                         .Select(n => new PlaylistDto
                                         {
                                             Id = n.Id,
                                             Title = n.Title,
                                             Songs = n.Songs,
                                             Image = n.Image, // Массив байт
                                             PublishedDate = n.Date,
                                             Link = n.Link
                                         })
                                         .ToListAsync();
            return playlistsList;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlaylist(int id, [FromBody] PlaylistDto playlistDto)
        {
            if (id != playlistDto.Id)
            {
                return BadRequest();
            }

            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist == null)
            {
                return NotFound();
            }

            playlist.Title = playlistDto.Title;
            playlist.Songs = playlistDto.Songs;
            playlist.Image = playlistDto.Image;
            playlist.Date = playlistDto.PublishedDate;
            playlist.Link = playlistDto.Link;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaylistExists(id))
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
        public async Task<IActionResult> DeletePlaylist(int id)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist == null)
            {
                return NotFound();
            }

            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool PlaylistExists(int id)
        {
            return _context.News.Any(e => e.Id == id);
        }
    }

    public class PlaylistDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<string> Songs { get; set; }
        public byte[]? Image { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string? Link { get; set; }
    }
}

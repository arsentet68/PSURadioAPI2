using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSURadioAPI2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PSURadioAPI2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly PsuradioContext _context;

        public MessagesController(PsuradioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
        {
            return await _context.Messages.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Message>> SendMessage(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMessages), new { id = message.Id }, message);
        }
    }
}

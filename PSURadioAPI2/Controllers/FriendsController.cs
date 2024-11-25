using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSURadioAPI2.Models;
using PSURadioAPI2.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSURadioAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        ApplicationContext db;
        public FriendsController(ApplicationContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Friend>>> Get()
        {
            return await db.Friends.ToListAsync();
        }

        // GET api/Friends/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Friend>> Get(int id)
        {
            Friend Friend = await db.Friends.FirstOrDefaultAsync(x => x.Id == id);
            if (Friend == null)
                return NotFound();
            return new ObjectResult(Friend);
        }

        // POST api/Friends
        [HttpPost]
        public async Task<ActionResult<Friend>> Post(Friend Friend)
        {
            if (Friend == null)
            {
                return BadRequest();
            }

            db.Friends.Add(Friend);
            await db.SaveChangesAsync();
            return Ok(Friend);
        }

        // PUT api/Friends/
        [HttpPut]
        public async Task<ActionResult<Friend>> Put(Friend Friend)
        {
            if (Friend == null)
            {
                return BadRequest();
            }
            if (!db.Friends.Any(x => x.Id == Friend.Id))
            {
                return NotFound();
            }

            db.Update(Friend);
            await db.SaveChangesAsync();
            return Ok(Friend);
        }

        // DELETE api/Friends/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Friend>> Delete(int id)
        {
            Friend Friend = db.Friends.FirstOrDefault(x => x.Id == id);
            if (Friend == null)
            {
                return NotFound();
            }
            db.Friends.Remove(Friend);
            await db.SaveChangesAsync();
            return Ok(Friend);
        }
    }
}
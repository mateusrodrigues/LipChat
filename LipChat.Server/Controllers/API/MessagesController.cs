using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using LipChat.Server.Data;
using LipChat.Library.Models;

namespace LipChat.Server.Controllers.API
{
    public class MessagesController : ApiController
    {
        private ChatDbContext db = new ChatDbContext();

        // GET: api/Messages
        [HttpGet]
        [Route("api/Messages")]
        public IEnumerable<Message> GetMessages(int? last)
        {
            if (last.HasValue)
            {
                var numMessages = db.Messages.Count();

                var result = db.Messages
                    .ToList()
                    .Skip(numMessages - last.Value);

                return result;
            }

            return db.Messages.ToList();
        }

        // GET: api/Messages/5
        [HttpGet]
        [ResponseType(typeof(Message))]
        public async Task<IHttpActionResult> GetMessage(Guid id)
        {
            Message message = await db.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            return Ok(message);
        }

        // POST: api/Messages
        [HttpPost]
        [ResponseType(typeof(Message))]
        public async Task<IHttpActionResult> PostMessage(Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Messages.Add(message);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = message.MessageID }, message);
        }

        // DELETE: api/Messages/5
        [HttpDelete]
        [ResponseType(typeof(Message))]
        public async Task<IHttpActionResult> DeleteMessage(Guid id)
        {
            Message message = await db.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            db.Messages.Remove(message);
            await db.SaveChangesAsync();

            return Ok(message);
        }

        // DELETE: api/Messages
        [HttpDelete]
        [Route("api/Messages")]
        public async Task<IHttpActionResult> DeleteAllMessages()
        {
            var messages = await db.Messages.ToListAsync();
            db.Messages.RemoveRange(messages);

            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MessageExists(Guid id)
        {
            return db.Messages.Count(e => e.MessageID == id) > 0;
        }
    }
}
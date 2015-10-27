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
using MessagingAPI.Models;
using Microsoft.AspNet.SignalR;
using MessagingAPI.Hubs;
using Microsoft.AspNet.Identity;

namespace MessagingAPI.Controllers
{
    public class MessageController : AuthorizedController<MessagingHub>
    {
        // GET: api/Message
        public IEnumerable<MessageBoardMessage> GetMessages()
        {
            return EfContext.Messages.ToList().Select<Message, MessageBoardMessage>(MessageFormat).ToList();
        }

        // GET: api/Message/5
        [ResponseType(typeof(MessageBoardMessage))]
        public async Task<IHttpActionResult> GetMessage(int id)
        {
            Message message = await EfContext.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            return Ok(MessageFormat(message));
        }

        // PUT: api/Message/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMessage(int id, Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != message.Id)
            {
                return BadRequest();
            }

            EfContext.Entry(message).State = EntityState.Modified;

            try
            {
                await EfContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Message
        [ResponseType(typeof(MessageBoardMessage))]
        public async Task<IHttpActionResult> PostMessage(Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            message.UserId = User.Identity.GetUserId<int>();

            EfContext.Messages.Add(message);

            try
            {
                await EfContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            var msgBoardmsg = MessageFormat(message);

            Hub.Clients.All.sendMessage(msgBoardmsg);

            return CreatedAtRoute("DefaultApi", new { id = message.Id }, msgBoardmsg);
        }

        private MessageBoardMessage MessageFormat(Message msg)
        {
            return new MessageBoardMessage
            {
                Id = msg.Id,
                Username = (msg.User != null ? msg.User.UserName : User.Identity.GetUserName()).Split('@')[0],
                When = msg.When,
                Content = msg.Content
            };
        }

        // DELETE: api/Message/5
        [ResponseType(typeof(Message))]
        public async Task<IHttpActionResult> DeleteMessage(int id)
        {
            Message message = await EfContext.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            EfContext.Messages.Remove(message);
            await EfContext.SaveChangesAsync();

            return Ok(message);
        }

        [Route("api/Messages/Clear")]
        [HttpDelete]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> ClearMessages()
        {
            await EfContext.Database.ExecuteSqlCommandAsync("TRUNCATE TABLE [Messages]");

            Hub.Clients.All.messagesCleared();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                EfContext.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MessageExists(int id)
        {
            return EfContext.Messages.Count(e => e.Id == id) > 0;
        }
    }
}
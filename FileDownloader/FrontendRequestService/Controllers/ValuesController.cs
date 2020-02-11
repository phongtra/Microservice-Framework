using FrontendRequestService.Models;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.MessagePublishCommon;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FrontendRequestService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly Message _message;

        public ValuesController(Message message)
        {
            _message = message;
        }
        // GET: api/<controller>
        [HttpGet]
        public void Get()
        {
            //Message.PublishMessage("test", "Fuckyou");
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public void Get(int id)
        {
            //Message.PublishMessage("test", "Fuckyou");
        }

        // POST api/<controller>
        [HttpPost]
        public void Post(Link link)
        {
            _message.PublishMessage(link.Value, "DownloadLink");
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, string value)
        {
            //Message.PublishMessage("test", "Fuckyou");
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            //Message.PublishMessage("test", "Fuckyou");
        }
    }
}

using FrontendRequestService.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FrontendRequestService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET: api/<controller>
        [HttpGet]
        public void Get()
        {
            Message.PublishMessage();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public void Get(int id)
        {
            Message.PublishMessage();
        }

        // POST api/<controller>
        [HttpPost]
        public void Post(Link link)
        {
            Message.PublishMessage(link);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, string value)
        {
            Message.PublishMessage();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Message.PublishMessage();
        }
    }
}

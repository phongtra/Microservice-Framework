using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatabaseAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace DatabaseAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public QuestionsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Questions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestions()
        {
            return await _context.Questions.ToListAsync();
        }
        [HttpGet("ok")]
        public OkObjectResult GetOk()
        {
            return Ok(200);
        }
        [HttpGet("titles")]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestionTitles()
        {
            var questions = await _context.Questions.Select(q => q.Title).ToListAsync();
            return Ok(questions);
        }
        
        [HttpGet("author/titles/{id}")]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestionTitles(int id)
        {
            var questions = await _context.Questions.Where(q => q.AuthorId == id).Select(q => q.Title).ToListAsync();
            return Ok(questions);
        }
        [Authorize(Roles = "USER")]
        // GET: api/Questions/5
        [HttpGet("author/{id}")]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestion(int id)
        {
            return await _context.Questions.Where(q => q.AuthorId == id).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetOneQuestion(int id)
        {
            var question = await _context.Questions.FindAsync(id);

            if (question == null)
            {
                return NotFound();
            }
            return question;
        }

        // PUT: api/Questions/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestion(int id, Question question)
        {
            if (id != question.Id)
            {
                return BadRequest();
            }

            _context.Entry(question).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
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

        // POST: api/Questions
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<ActionResult<Question>> PostQuestion(Question question)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuestion", new { id = question.Id }, question);
        }

        // DELETE: api/Questions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Question>> DeleteQuestion(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

            return question;
        }

        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }
    }
}

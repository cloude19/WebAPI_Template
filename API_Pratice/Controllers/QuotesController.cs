using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Pratice.Data;
using API_Pratice.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Pratice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotesController : ControllerBase
    {
        private QuotesDbContext _quotesDbContext;

        /*Not sure where yet but the dbcontext object with the options is being injected here.
         this constructor uses the QuotesDbContext referenced in the (Startup.cs)ConfigureServices to 
         resolve this dependency(Also most likely uses IOC)*/
        public QuotesController(QuotesDbContext quotesDbContext)
        {
            _quotesDbContext = quotesDbContext;
        }

        //IaActionResult is going to return the http status along with the results of the query if needed. 
        // GET: api/Quotes
        [HttpGet]
        public IActionResult Get()
        {
            // return _quotesDbContext.Quotes;
            return Ok(_quotesDbContext.Quotes);
        }

        // GET: api/Quotes/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
           var quote = _quotesDbContext.Quotes.Find(id);
            if(quote == null)
            {
                return NotFound("Record not found");
            }
           return Ok(quote);
        }

        // POST: api/Quotes
        [HttpPost]
        public IActionResult Post([FromBody] Quote quote)
        {
            _quotesDbContext.Quotes.Add(quote);
            _quotesDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT: api/Quotes/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Quote quote)
        {
            //get row and pass in changes
            var targetEntity = _quotesDbContext.Quotes.Find(id);
            if (targetEntity == null)
            {
                return NotFound("Record not found.");
            }
            else
            {
                targetEntity.Title = quote.Title;
                targetEntity.Author = quote.Author;
                targetEntity.Description = quote.Description;
                targetEntity.Type = quote.Type;
                targetEntity.CreatedAt = quote.CreatedAt;

                //update row with changes
                _quotesDbContext.Quotes.Update(targetEntity);
                _quotesDbContext.SaveChanges();
            }

            return Ok(@"record updated succefully.");
        }

        // DELETE: api/Quotes/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var targetEntity = _quotesDbContext.Quotes.Find(id);
            if (targetEntity == null)
            {
                return NotFound("Record not found");
            }
            else
            {
                _quotesDbContext.Quotes.Remove(targetEntity);
                _quotesDbContext.SaveChanges();
            }

            return Ok("Record deleted.");
        }
    }
}

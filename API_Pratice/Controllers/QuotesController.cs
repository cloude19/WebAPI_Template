﻿using System;
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
        public IActionResult Get(string sort = null)
        {
            /*This one whould return  select * from Quotes */
            //IEnumerable<Quote>
            IQueryable<Quote> quotes;
            switch (sort)
            {
                case "desc":
                    quotes = _quotesDbContext.Quotes.OrderByDescending(q => q.CreatedAt);
                    break;
                case "asc":
                    quotes = _quotesDbContext.Quotes.OrderBy(q => q.CreatedAt);
                    break;
                default:
                    quotes = _quotesDbContext.Quotes;
                    break;
            }
            // return _quotesDbContext.Quotes;
            return Ok(quotes);
        }

        /// <summary>
        /// Will implement paging for the API.(Sending data in chunks
        /// based off what page the user is currently on)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public IActionResult PagingQuote(int? pageNumber, int? pageSize)
        {
            var quotes = _quotesDbContext.Quotes;

            //check argumemnts for null if null 
            var currentPageNumber = pageNumber != null ? (int)pageNumber : 1;
            var currentPageSize = pageSize != null ? (int)pageSize : 3;

            /*algorithm will skip and display records based off the 
             page the client is currently viewing.*/
            return Ok(quotes.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
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

        //Route: api/Quotes/TheAction/1?
        [HttpGet("[action]/{id}")]
        public int TheAction(int id = 0)
        {
            return id;
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

        /// <summary>
        /// Search database for sepcific type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public IActionResult SearchQuote(string type)
        {
          var quotes =  _quotesDbContext.Quotes.Where(q => q.Type.StartsWith(type));
          return Ok(quotes);
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

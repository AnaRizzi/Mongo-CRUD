using Microsoft.AspNetCore.Mvc;

namespace Crud_Mongo.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly MongoConnection _mongo;

        public BooksController()
        {
            _mongo = new MongoConnection();
        }

        // GET: api/<BooksController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var books = await _mongo.GetAll();

                return Ok(books);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<BooksController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var book = await _mongo.GetById(id);

                return Ok(book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<BooksController>/5
        [HttpGet("author/{name}")]
        public async Task<IActionResult> GetByAuthor(string name)
        {
            try
            {
                var books = await _mongo.GetByAuthor(name);

                return Ok(books);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<BooksController>/5
        [HttpGet("orderbyname")]
        public async Task<IActionResult> GetAlphabetical()
        {
            try
            {
                var books = await _mongo.GetAlfabeticalOrder();

                return Ok(books);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<BooksController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Books book)
        {
            try
            {
                await _mongo.InsertOne(book);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<BooksController>
        [HttpPost("insertmany")]
        public async Task<IActionResult> PostMany([FromBody] IEnumerable<Books> books)
        {
            try
            {
                await _mongo.InsertMany(books);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<BooksController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Books book)
        {
            try
            {
                await _mongo.Update(id, book);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<BooksController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _mongo.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

using BooksWebApp.Utils;
using ConceptArchitect.BookManagement;
using ConceptArchitect.BookManagement.Repositories.AdoNet;
using Microsoft.AspNetCore.Mvc;

namespace BooksWebApp.Controllers
{
    [ApiController]
    [Route("api/authors")]
    [ExceptionMapper(ExceptionType = typeof(EntityNotFoundException<string>), HttpStatus = 404)]
    [ExceptionMapper(ExceptionType = typeof(DuplicateEntitityException<string>), HttpStatus = 400)]
    public class AuthorsController : Controller
    {
        IAuthorService service;
        public AuthorsController(IAuthorService service)
        {
            this.service = service;
        }


        [HttpGet("{id}", Name = "GetAuthorRoute")]

        public IActionResult GetAuthorById(string id)
        {
            var author = service.GetAuthorById(id);
            return Ok(author);  //Ok result is 200
        }

        [HttpPost]

        public IActionResult AddAuthor([FromBody] Author author)
        {

            service.AddAuthor(author);
            return Created(
                    Url.RouteUrl("GetAuthorRoute", new { id = author.Id }),
                    author);

        }


        [HttpPut("{id}")]
        public IActionResult UpdateAuthor(string id, [FromBody] Author author)
        {
            service.SaveAuthor(author);
            return Accepted(author);
        }


        [HttpDelete("{authorId}")]
        public IActionResult DeleteAuthor(string authorId)
        {
            service.RemoveAuthor(authorId);
            return NoContent();

        }





        [HttpGet("")]
        public List<Author> GetAllAuthors()
        {
            return service.GetAll();
        }







    }
}
using ConceptArchitect.BookManagement;
using Microsoft.AspNetCore.Mvc;

namespace BooksWebApp.Controllers
{
    [ApiController]
    [Route("api/authors")]  // common route for all methods in this controller
    // api controller tels that we should use route mapping instead of following controller/action/id
    public class AuthorsApiController :Controller
    {
        IAuthorService service; // IAuthorManager -> IAuthorService
        public AuthorsApiController(IAuthorService service)
        {

            this.service = service;
        }


        [HttpGet("")] //This URI is relative to Route ggiven at controller level
        // api/authors/

        public List<Author> GetAllAuthors()
        {
            return service.GetAll();
        }

        [HttpGet("{id}",Name ="GetAuthorRouteV1")] //api/authors/id exampple api/authors/ponnam-venu
        public IActionResult GetAuthorById(string id)
        {
            try
            {
                var author = service.GetAuthorById(id);
                return Ok(author); // OK 200

            }
            catch 
            {
                return NotFound();//404 error
            }
            
        }


        [HttpPost]// creating
        public IActionResult AddAuthor([FromBody] Author author)
        {
            try 
            {
                service.AddAuthor(author);
                return Created(
                    Url.RouteUrl("GetAuthorRoute",new { id = author.Id }), // this is where object can be accessed using GET
                    author);
                
            } 
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            
            }
        }




        [HttpDelete("{authorId}")] // for deleting
        public IActionResult DeleteAuthor(string authorId)
        {
            try { service.RemoveAuthor(authorId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);

            }

        }

        [HttpPut("{id}")] // for updating
        public IActionResult UpdateAuthor(string id,[FromBody] Author author)
        {
            try {
                service.SaveAuthor(author);
                return Accepted(author);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        
    }
   
}

using BooksWebApp.ViewModels;
using ConceptArchitect.BookManagement;
using Microsoft.AspNetCore.Mvc;

namespace BooksWebApp.Controllers
{
    public class AuthorController : Controller
    {
       
        IAuthorService authorManager;
        public AuthorController(IAuthorService authorManager)
        {
            this.authorManager = authorManager;
        }

        public ViewResult Index()
        {
            var authors = authorManager.GetAll();
            return View(authors);
        }
        public ActionResult Details(string id)
        {
            // var author = authors.FirstOrDefault(a => a.AuthorId == id);

            return ShowSingleAuthorView(id);
        }



        // Assignment on 24-08-2022
        public ActionResult Delete(string id)
        {
           authorManager.RemoveAuthor(id);
            return RedirectToAction("Index");
        }
        
        //httpGet
        public ActionResult Edit(string id)
        {
            //Business Model
            var author = authorManager.GetAuthorById(id);

            //BusinessModel --> ViewModel

            //var vm = new AuthorEditVM()
            //{
            //    Id = author.Id,
            //    Name = author.Name,
            //    Biography = author.Biography,
            //    Photo = author.Photo,
            //    Tags = string.Join(',', author.Tags)
            //};


            //var vm = new AuthorEditVM().Copy(author, (vm, m) =>
            //{
            //    vm.Tags = String.Join(',', author.Tags);
            //});

            var vm = author.ToAuthorVM();

            return View(vm);
        }

        [HttpPost]
        public ActionResult Edit(AuthorEditVM authorVM, string tags)
        {
            //VM --> Business Model
            //var author = new Author()
            //{
            //    Id = authorVM.Id,
            //    Name = authorVM.Name,
            //    Photo = authorVM.Photo,
            //    Biography = authorVM.Biography,
            //    Tags = authorVM.Tags.Split(',').ToList()
            //};

            //var author = new Author().Copy(authorVM, (m, vm) => m.Tags = vm.Tags.Split(',').ToList());

            var author = authorVM.ToAuthor();

            authorManager.SaveAuthor(author);
            return RedirectToAction("Index");
        }

        private ActionResult ShowSingleAuthorView(string id)
        {
            var author = authorManager.GetAuthorById(id);
            return View(author);
        }
        public ActionResult Update(Author author)
        {
            authorManager.SaveAuthor(author);
            return RedirectToAction("Index");
        }
        // here are Overloaded Create methods c# allows but MVC ambigious so eliminate ambigious we should seprated by httpget/post
        [HttpGet]
        public ActionResult Create()
        {
            // return View(new Author());
            return View(new AuthorEditVM());
        }

        [HttpPost]
        public ActionResult Create(AuthorEditVM authorVM, string tags)
        {
            //author.Tags = tags.Split(',').ToList();

            // authors.Add(author);

            //var author = new Author()
            //{
            //    Id=authorVM.Id,
            //    Name=authorVM.Name,
            //    Photo=authorVM.Photo,
            //    Biography=authorVM.Biography,
            //    Tags=authorVM.Tags.Split(',').ToList()
            //};

            //var author = new Author().Copy(authorVM, (m, vm) => m.Tags = vm.Tags.Split(',').ToList());

            var author = authorVM.ToAuthor();

            authorManager.AddAuthor(author);


            return RedirectToAction("Index");
        }//create works same as add author so create method is overloaded







        [NonAction]
        public ActionResult AddAuthor(Author author, string tags)
        {
            ApplyTags(author, tags);

            //authors.Add(author);

            authorManager.AddAuthor(author);


            return RedirectToAction("Index");
        }

        public static void ApplyTags(Author author, string tags)
        {
            author.Tags = tags.Split(',').ToList();
        }
        [NonAction]
        public ActionResult AddAuthor2(string authorId,string name,string biography,string photo,string tags)
        {
            var author = new Author()
            {
                Id = authorId,
                Name = name,
                Biography = biography,
                Photo = photo,
                Tags = tags.Split(",").ToList()
            };
            //authors.Add(author);
            authorManager.AddAuthor(author);
            return RedirectToAction("Index");
        }
        [NonAction]
        public ActionResult AddAuthor1()
        {
            var author = new Author()
            {
                Id = Request.Form["Id"],
                Name = Request.Form["name"],
                Biography = Request.Form["Biography"],
                Photo = Request.Form["Photo"],
                Tags = Request.Form["Tags"].ToString().Split(",").ToList()
            };
            // authors.Add(author);

            authorManager.AddAuthor(author);
            return RedirectToAction("Index");
        }

    }
}

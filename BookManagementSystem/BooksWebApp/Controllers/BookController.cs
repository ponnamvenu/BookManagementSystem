using ConceptArchitect.BookManagement;
using Microsoft.AspNetCore.Mvc;

namespace BooksWebApp.Controllers
{
    public class BookController : Controller
    {
        IBookService bookManager;
        public BookController(IBookService bookManager)
        { 
        this.bookManager = bookManager;
        }
        public IActionResult Index()
        {
            var books = bookManager.GetAll();
            return View(books);
        }
        private ActionResult ShowSingleBookView(string id)
        {
            var book = bookManager.GetBookByid(id);
            return View(book);
        }
        public ActionResult Details(string id)
        {
            return ShowSingleBookView(id);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View(new Book());
        }
        [HttpPost]
        public ActionResult Create(Book book)
        {
            bookManager.AddBook(book);
            return RedirectToAction("Index");
        }

        //---------------------------------------
        // Assignment on 28-08-2022
        public ActionResult Delete(string id)
        {
           bookManager.RemoveBook(id);
            return RedirectToAction("Index");
        }
        /* public ActionResult Delete(string id, object sender, EventArgs e)
         {
             string confirmValue = Request.Form["confirm_value"];
             if (confirmValue == "Yes")
             {
                 bookManager.RemoveBook(id);
                 //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked YES!')", true);
             }
             else
             {
                 //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
             }

             return RedirectToAction("Index");
         }*/

        //httpGet
        public ActionResult Edit(string id)
        {
            //Business Model
            var book = bookManager.GetBookByid(id);
            return View(book);
        }

        [HttpPost]
        public ActionResult Edit(Book book)
        {/*
            bookManager.SaveBook(book);

            return RedirectToAction("Index");*/
            try
            {
                bookManager.SaveBook(book);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //   return View("Exception",book);
                ModelState.AddModelError("authorId", "Invalid Author Id");
                return View(book);
            }
           
        }

    }
}

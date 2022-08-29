using ConceptArchitect.BookManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BooksWebApp.Controllers
{
    public class UserController : Controller
    {
        IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        // GET: UserController
        public ActionResult Index()
        {
            var users = userService.GetAll();
            return View();
        }

        // GET: UserController/Details/5
        public ActionResult Details(string id)
        {
            var user = userService.GetUserByid(id);
            return View(user);
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View( new User());
        }

        // POST: UserController/Create
        [HttpPost]
        public ActionResult Create(User user)
        {
            userService.AddUser(user);
            return RedirectToAction("Index");
        }
        /* [ValidateAntiForgeryToken]
         public ActionResult Create(IFormCollection collection)
         {
             try
             {
                 return RedirectToAction(nameof(Index));
             }
             catch
             {
                 return View();
             }
         }*/

        // GET: UserController/Edit/5
        public ActionResult Edit(string id)
        {
            var user=userService.GetUserByid(id);
            return View(user);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            try
            {
                userService.SaveUser(user);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(); //Need to Change---------------------------------------
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(string id)
        {
            var user = userService.GetUserByid(id);
            return View(user);
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(User user)
        {
            userService.RemoveUser(user.Email);
            return RedirectToAction("Index");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SimpleMVC.Controllers
{
    public class AlexaController:Controller
    {
        public ViewResult Hello()
        {
            return new ViewResult();
        }

        public ViewResult TellTheTime()
        {
            return new ViewResult();
        }

        public ViewResult Today()
        {
            var date = DateTime.Now;


            return View("DateTimeView", date);
        }

        public ViewResult Tomorrow()
        {
            var date=DateTime.Now.AddDays(1);

            return View("DateTimeView", date);
        }
    }
}

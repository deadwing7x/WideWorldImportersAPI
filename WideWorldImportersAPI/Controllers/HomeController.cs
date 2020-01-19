namespace WebApplication1.Controllers
{
    #region Usings
    using System.Web.Mvc;
    #endregion

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}

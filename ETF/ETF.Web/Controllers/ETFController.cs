namespace ETF.Web.Controllers
{
    using System.Web.Mvc;

    public class EtfController : Controller
    {
        // GET: ETF
        public ActionResult Dashboard()
        {
            return this.View("_Dashboard");
        }
    }
}
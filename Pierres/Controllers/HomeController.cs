using Microsoft.AspNetCore.Mvc;
using Pierres.Models;
using System.Linq;

namespace Pierres.Controllers
{
  public class HomeController : Controller
  {
    private readonly PierresContext _db;

    public HomeController(PierresContext db)
    {
      _db = db;
    }
    public ActionResult Index()
    {
      ViewBag.Treats = _db.Treats.ToList();
      ViewBag.Flavors = _db.Flavors.ToList();
      ViewBag.PageTitle = "Pierre's Bakery!";
      return View();
    }
  }
}
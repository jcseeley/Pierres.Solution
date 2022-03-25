using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pierres.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pierres.Controllers
{
    public class TreatsController : Controller
  {
    private readonly PierresContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public TreatsController(UserManager<ApplicationUser> userManager, PierresContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    public ActionResult Index()
    {
      List<Treat> treats = _db.Treats.ToList();
      ViewBag.PageTitle = "Treats!";
      return View(treats);
    }

    public async Task<ActionResult> Create()
    {
      string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      ViewBag.PageTitle = "Add Treat!";
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Treat treat)
    {
      string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      treat.User = currentUser;
      Treat check = _db.Treats.Where(check => check.Name == treat.Name).FirstOrDefault();
      if (check != null)
      {
        ViewBag.Error = "exist";
        return View();
      }
      else
      {
        _db.Treats.Add(treat);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }

    public ActionResult Details(int id)
    {
      Treat treat = _db.Treats
        .Include(treat => treat.JoinEntities)
        .ThenInclude(join => join.Flavor)
        .FirstOrDefault(treat => treat.TreatId == id);
        ViewBag.PageTitle = "Treat Details!";
      return View(treat);
    }

    public async Task<ActionResult> Edit(int id)
    {
      string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      Treat treat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      ViewBag.PageTitle = "Edit Treat!";
      return View(treat);
    }

    [HttpPost]
    public async Task<ActionResult> Edit(Treat treat)
    {
      string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      Treat check = _db.Treats.Where(check => check.Name == treat.Name && check.TreatId != treat.TreatId).FirstOrDefault();
      if (check != null)
      {
        ViewBag.Error = "exist";
        return View(treat);
      }
      else
      {
        _db.Entry(treat).State = EntityState.Modified;
        _db.SaveChanges();
        return RedirectToAction("Details", new { id = treat.TreatId });
      }
    }

    public async Task<ActionResult> AddFlavor(int id)
    {
      string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      Treat treat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Name");
      ViewBag.PageTitle = "Add Treat Flavor!";
      return View(treat);
    }

    [HttpPost]
    public async Task<ActionResult> AddFlavor(Treat treat, int FlavorId)
    {
      string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      Taste check = _db.Tastes.Where(check => check.Treat.TreatId == treat.TreatId && check.Flavor.FlavorId == FlavorId).FirstOrDefault();
      if (check != null)
      {
        ViewBag.Error = "exist";
        return View(treat);
      }
      else
      {
        _db.Tastes.Add(new Taste() { FlavorId = FlavorId, TreatId = treat.TreatId });
        _db.SaveChanges();
        return RedirectToAction("Details", new { id = treat.TreatId });
      }
    }

    [HttpPost]
    public async Task<ActionResult> DeleteFlavor(int joinId)
    {
      string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      Taste joinEntry = _db.Tastes.FirstOrDefault(entry => entry.TasteId == joinId);
      _db.Tastes.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = joinEntry.TreatId });
    }
    
    public async Task<ActionResult> Delete(int id)
    {
      string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      Treat treat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      ViewBag.PageTitle = "Delete Treat!";
      return View(treat);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
      string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      Treat treat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      _db.Treats.Remove(treat);
      foreach (Taste taste in treat.JoinEntities)
      {
        _db.Tastes.Remove(taste);
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}
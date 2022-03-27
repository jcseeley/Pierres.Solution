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
    public class FlavorsController : Controller
  {
    private readonly PierresContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public FlavorsController(UserManager<ApplicationUser> userManager, PierresContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    public ActionResult Index()
    {
      List<Flavor> flavors = _db.Flavors.ToList();
      ViewBag.PageTitle = "Flavors!";
      return View(flavors);
    }

    public async Task<ActionResult> Create()
    {
      string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      ViewBag.PageTitle = "Add Flavor!";
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Flavor flavor)
    {
      string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      flavor.User = currentUser;
      Flavor check = _db.Flavors.Where(check => check.Name == flavor.Name).FirstOrDefault();
      if (check != null)
      {
        ViewBag.Error = "exist";
        ViewBag.PageTitle = "Add Flavor!";
        return View();
      }
      else
      {
        _db.Flavors.Add(flavor);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }

    public ActionResult Details(int id)
    {
      Flavor flavor = _db.Flavors
        .Include(flavor => flavor.JoinEntities)
        .ThenInclude(join => join.Treat)
        .FirstOrDefault(flavor => flavor.FlavorId == id);
        ViewBag.PageTitle = "Flavor Details!";
      return View(flavor);
    }

    public async Task<ActionResult> Edit(int id)
    {
      string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      Flavor flavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
      ViewBag.TreatId = new SelectList(_db.Treats, "TreatId", "Name");
      ViewBag.PageTitle = "Edit Flavor!";
      return View(flavor);
    }

    [HttpPost]
    public async Task<ActionResult> Edit(Flavor flavor)
    {
      string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      Flavor check = _db.Flavors.Where(check => check.Name == flavor.Name && check.FlavorId != flavor.FlavorId).FirstOrDefault();
      if (check != null)
      {
        ViewBag.Error = "exist";
        ViewBag.PageTitle = "Edit Flavor!";
        return View(flavor);
      }
      else
      {
        _db.Entry(flavor).State = EntityState.Modified;
        _db.SaveChanges();
        return RedirectToAction("Details", new { id = flavor.FlavorId });
      }
    }

    public async Task<ActionResult> AddTreat(int id)
    {
      string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      Flavor flavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
      ViewBag.Treats = _db.Treats.ToList();
      ViewBag.TreatId = new SelectList(_db.Treats, "TreatId", "Name");
      ViewBag.PageTitle = "Add Treat to Flavor!";
      return View(flavor);
    }

    [HttpPost]
    public async Task<ActionResult> AddTreat(Flavor flavor, int TreatId)
    {
      string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      Taste check = _db.Tastes.Where(check => check.Flavor.FlavorId == flavor.FlavorId && check.Treat.TreatId == TreatId).FirstOrDefault();
      if (check != null)
      {
        ViewBag.Error = "exist";
        ViewBag.PageTitle = "Add Treat to Flavor!";
        ViewBag.Treats = _db.Treats.ToList();
        ViewBag.TreatId = new SelectList(_db.Treats, "TreatId", "Name");
        return View(flavor);
      }
      else
      {
        _db.Tastes.Add(new Taste() { TreatId = TreatId, FlavorId = flavor.FlavorId });
        _db.SaveChanges();
        return RedirectToAction("Details", new { id = flavor.FlavorId });
      }
    }

    [HttpPost]
    public async Task<ActionResult> DeleteTreat(int joinId)
    {
      string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      Taste joinEntry = _db.Tastes.FirstOrDefault(entry => entry.TasteId == joinId);
      _db.Tastes.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = joinEntry.FlavorId });
    }
    
    public async Task<ActionResult> Delete(int id)
    {
      string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      Flavor flavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
      ViewBag.PageTitle = "Delete Flavor!";
      return View(flavor);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
      string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      Flavor flavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
      _db.Flavors.Remove(flavor);
      foreach (Taste taste in flavor.JoinEntities)
      {
        _db.Tastes.Remove(taste);
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public async Task<ActionResult> DeleteAll()
    {
      string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      return View();
    }

    [HttpPost, ActionName("DeleteAll")]
    public async Task<ActionResult> DeleteAllTreats()
    {
      string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      foreach(Flavor flavor in _db.Flavors)
      {
        _db.Flavors.Remove(flavor);
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}
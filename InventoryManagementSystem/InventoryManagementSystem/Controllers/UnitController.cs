using InventoryManagementSystem.Interfaces;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Tools;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace InventoryManagementSystem.Controllers
{
    public class UnitController : Controller
    {
        private readonly IUnit _unitRepo;

        public UnitController(IUnit unitRepo)
        {
            _unitRepo = unitRepo;
        }

        public IActionResult Index(string sortExpression = "", string searchText="", int pg=1, int pageSize=5)
        {
            var sortModel = new SortModel();
            sortModel.AddColumn("description");
            sortModel.AddColumn("name");
            
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"]=sortModel;
            ViewBag.SearchText= searchText;
            
            var units = _unitRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, searchText, pg,pageSize);

            var pager = new PagerModel(units.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;

            return View(units);
        }
        public IActionResult Create()
        {
            var unit= new Unit();
            return View(unit);
        }
        [HttpPost]
        public IActionResult Create(Unit unit)
        {
            _unitRepo.Create(unit);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            var unit = _unitRepo.GetUnit(id);
            return View(unit);
        }
        [HttpPost]
        public IActionResult Edit(Unit unit)
        {
            _unitRepo.Edit(unit);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var unit = _unitRepo.GetUnit(id);
            return View(unit);
        }
        [HttpPost]
        public IActionResult Delete(Unit unit)
        {
            _unitRepo.Delete(unit);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Details(int id)
        {
            var unit = _unitRepo.GetUnit(id);
            return View(unit);
        }
    }
}

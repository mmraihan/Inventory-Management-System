using InventoryManagementSystem.Data;
using InventoryManagementSystem.Interfaces;
using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult Index(string sortExpression = "")
        {
            ViewData["SortParamName"] = "name";
            ViewData["SortParamDesc"] = "description";

            ViewData["SortIconName"] = "";
            ViewData["SortIconDesc"] = "";

            var sortModel = new SortModel();

            switch (sortExpression.ToLower())
            {
                case "name_desc":
                    sortModel.SortedOrder = SortOrder.Descending;
                    sortModel.SortedProperty = "name";
                    ViewData["SortParamName"] = "name";
                    ViewData["SortIconName"] = "fa fa-arrow-up";
                    break;

                case "description":
                    sortModel.SortedOrder = SortOrder.Ascending;
                    sortModel.SortedProperty = "description";
                    ViewData["SortParamDesc"] = "description_desc";
                    ViewData["SortIconDesc"] = "fa fa-arrow-down";
                    break;

                case "description_desc":
                    sortModel.SortedOrder = SortOrder.Descending;
                    sortModel.SortedProperty = "description";
                    ViewData["SortParamDesc"] = "description";
                    ViewData["SortIconDesc"] = "fa fa-arrow-up";
                    break;

                default:
                    sortModel.SortedOrder = SortOrder.Ascending;
                    sortModel.SortedProperty = "name";
                    ViewData["SortParamName"] = "name_desc";
                    ViewData["SortIconName"] = "fa fa-arrow-down";
                    break;
            }
            var units = _unitRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder);
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

using InventoryManagementSystem.Interfaces;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace InventoryManagementSystem.Controllers
{
    [Authorize]
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

            TempData["CurrentPage"] = pg;

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

            bool bolret = false;
            string errMessage = "";
            try
            {
                if (unit.Description.Length < 4 || unit.Description == null)
                    errMessage = "Unit Description Must be atleast 4 Characters";

                if (_unitRepo.IsUnitNameExists(unit.Name) == true)
                    errMessage = errMessage + " " + " Unit Name " + unit.Name + " Exists Already";

                if (errMessage == "")
                {
                    unit = _unitRepo.Create(unit);
                    bolret = true;
                }
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }
            if (bolret == false)
            {
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(unit);
            }
            else
            {
                TempData["SuccessMessage"] = unit.Name + " Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }
           
        
        public IActionResult Edit(int id)
        {
            var unit = _unitRepo.GetUnit(id);
            TempData.Keep();
            return View(unit);
        }

        [HttpPost]
        public IActionResult Edit(Unit unit)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                if (unit.Description.Length < 4 || unit.Description == null)
                    errMessage = "Unit Description Must be atleast 4 Characters";

                if (_unitRepo.IsUnitNameExists(unit.Name, unit.Id) == true)
                    errMessage = errMessage + "Unit Name " + unit.Name + " Already Exists";

                if (errMessage == "")
                {
                    unit = _unitRepo.Edit(unit);
                    TempData["SuccessMessage"] = unit.Name + " Unit Saved Successfully";
                    bolret = true;
                }
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }



            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];


            if (bolret == false)
            {
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(unit);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        
        }

        public IActionResult Delete(int id)
        {
            var unit = _unitRepo.GetUnit(id);
            TempData.Keep();
            return View(unit);
        }
        [HttpPost]
        public IActionResult Delete(Unit unit)
        {
            try
            {
                unit=_unitRepo.Delete(unit);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(unit);

            }


            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];

            TempData["SuccessMessage"] = unit.Name + " Deleted Successfully";

            return RedirectToAction(nameof(Index), new { pg = currentPage });

        }
        public IActionResult Details(int id)
        {
            var unit = _unitRepo.GetUnit(id);
            return View(unit);
        }
    }
}

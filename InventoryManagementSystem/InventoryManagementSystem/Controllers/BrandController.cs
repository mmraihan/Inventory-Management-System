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
    public class BrandController : Controller
    {
        private readonly IBrand _brandRepo;
        public BrandController(IBrand repo)
        {
            _brandRepo = repo;
        }

        public IActionResult Index(string sortExpression = "", string searchText="", int pg=1, int pageSize=5)
        {
            var sortModel = new SortModel();
            sortModel.AddColumn("description");
            sortModel.AddColumn("name");
            
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"]=sortModel;
            ViewBag.SearchText= searchText;
            
            var units = _brandRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, searchText, pg,pageSize);

            var pager = new PagerModel(units.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;

            TempData["CurrentPage"] = pg;

            return View(units);
        }
        public IActionResult Create()
        {
            var brand= new Brand();
            return View(brand);
        }
        [HttpPost]
        public IActionResult Create(Brand brand)
        {

            bool bolret = false;
            string errMessage = "";
            try
            {
                if (brand.Description.Length < 4 || brand.Description == null)
                    errMessage = "Brand Description Must be atleast 4 Characters";

                if (_brandRepo.IsItemExists(brand.Name) == true)
                    errMessage = errMessage + " " + " Brand Name " + brand.Name + " Exists Already";

                if (errMessage == "")
                {
                    brand = _brandRepo.Create(brand);
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
                return View(brand);
            }
            else
            {
                TempData["SuccessMessage"] = brand.Name + " Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }                  
        public IActionResult Edit(int id)
        {
            var brand = _brandRepo.GetItem(id);
            TempData.Keep();
            return View(brand);
        }

        [HttpPost]
        public IActionResult Edit(Brand brand)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                if (brand.Description.Length < 4 || brand.Description == null)
                    errMessage = "Brand Description Must be atleast 4 Characters";

                if (_brandRepo.IsItemExists(brand.Name, brand.Id) == true)
                    errMessage = errMessage + "Brand Name " + brand.Name + " Already Exists";

                if (errMessage == "")
                {
                    brand = _brandRepo.Edit(brand);
                    TempData["SuccessMessage"] = brand.Name + " Brand Saved Successfully";
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
                return View(brand);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        
        }

        public IActionResult Delete(int id)
        {
            var brand = _brandRepo.GetItem(id);
            TempData.Keep();
            return View(brand);
        }
        [HttpPost]
        public IActionResult Delete(Brand brand)
        {
            try
            {
                brand= _brandRepo.Delete(brand);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(brand);

            }


            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];

            TempData["SuccessMessage"] = brand.Name + " Deleted Successfully";

            return RedirectToAction(nameof(Index), new { pg = currentPage });

        }
        public IActionResult Details(int id)
        {
            var brand = _brandRepo.GetItem(id);
            return View(brand);
        }
    }
}

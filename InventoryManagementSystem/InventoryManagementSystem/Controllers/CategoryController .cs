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
    public class CategoryController : Controller
    {
        private readonly ICategory _categoryRepo;
        public CategoryController(ICategory repo)
        {
            _categoryRepo = repo;
        }

        public IActionResult Index(string sortExpression = "", string searchText="", int pg=1, int pageSize=5)
        {
            var sortModel = new SortModel();
            sortModel.AddColumn("description");
            sortModel.AddColumn("name");
            
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"]=sortModel;
            ViewBag.SearchText= searchText;
            
            var units = _categoryRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, searchText, pg,pageSize);

            var pager = new PagerModel(units.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;

            TempData["CurrentPage"] = pg;

            return View(units);
        }
        public IActionResult Create()
        {
            var category= new Category();
            return View(category);
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {

            bool bolret = false;
            string errMessage = "";
            try
            {
                if (category.Description.Length < 4 || category.Description == null)
                    errMessage = "Category Description Must be atleast 4 Characters";

                if (_categoryRepo.IsItemExists(category.Name) == true)
                    errMessage = errMessage + " " + " Category Name " + category.Name + " Exists Already";

                if (errMessage == "")
                {
                    category = _categoryRepo.Create(category);
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
                return View(category);
            }
            else
            {
                TempData["SuccessMessage"] = category.Name + " Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }                  
        public IActionResult Edit(int id)
        {
            var category = _categoryRepo.GetItem(id);
            TempData.Keep();
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                if (category.Description.Length < 4 || category.Description == null)
                    errMessage = "Category Description Must be atleast 4 Characters";

                if (_categoryRepo.IsItemExists(category.Name, category.Id) == true)
                    errMessage = errMessage + "Category Name " + category.Name + " Already Exists";

                if (errMessage == "")
                {
                    category = _categoryRepo.Edit(category);
                    TempData["SuccessMessage"] = category.Name + " Category Saved Successfully";
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
                return View(category);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        
        }

        public IActionResult Delete(int id)
        {
            var category = _categoryRepo.GetItem(id);
            TempData.Keep();
            return View(category);
        }
        [HttpPost]
        public IActionResult Delete(Category category)
        {
            try
            {
                category= _categoryRepo.Delete(category);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(category);

            }


            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];

            TempData["SuccessMessage"] = category.Name + " Deleted Successfully";

            return RedirectToAction(nameof(Index), new { pg = currentPage });

        }
        public IActionResult Details(int id)
        {
            var category = _categoryRepo.GetItem(id);
            return View(category);
        }
    }
}

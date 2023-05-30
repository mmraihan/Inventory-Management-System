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
    public class ProductProfileController : Controller
    {
        private readonly IProductProfile _productProfileRepo;
        public ProductProfileController(IProductProfile repo)
        {
            _productProfileRepo = repo;
        }

        public IActionResult Index(string sortExpression = "", string searchText="", int pg=1, int pageSize=5)
        {
            var sortModel = new SortModel();
            sortModel.AddColumn("description");
            sortModel.AddColumn("name");
            
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"]=sortModel;
            ViewBag.SearchText= searchText;
            
            var units = _productProfileRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, searchText, pg,pageSize);

            var pager = new PagerModel(units.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;

            TempData["CurrentPage"] = pg;

            return View(units);
        }
        public IActionResult Create()
        {
            var productProfile= new ProductProfile();
            return View(productProfile);
        }
        [HttpPost]
        public IActionResult Create(ProductProfile productProfile)
        {

            bool bolret = false;
            string errMessage = "";
            try
            {
                if (productProfile.Description.Length < 4 || productProfile.Description == null)
                    errMessage = "Product Profile Description Must be atleast 4 Characters";

                if (_productProfileRepo.IsItemExists(productProfile.Name) == true)
                    errMessage = errMessage + " " + " Product Profile Name " + productProfile.Name + " Exists Already";

                if (errMessage == "")
                {
                    productProfile = _productProfileRepo.Create(productProfile);
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
                return View(productProfile);
            }
            else
            {
                TempData["SuccessMessage"] = productProfile.Name + " Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }                  
        public IActionResult Edit(int id)
        {
            var productProfile = _productProfileRepo.GetItem(id);
            TempData.Keep();
            return View(productProfile);
        }

        [HttpPost]
        public IActionResult Edit(ProductProfile productProfile)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                if (productProfile.Description.Length < 4 || productProfile.Description == null)
                    errMessage = "Product Profile Description Must be atleast 4 Characters";

                if (_productProfileRepo.IsItemExists(productProfile.Name, productProfile.Id) == true)
                    errMessage = errMessage + "Product Profile Name " + productProfile.Name + " Already Exists";

                if (errMessage == "")
                {
                    productProfile = _productProfileRepo.Edit(productProfile);
                    TempData["SuccessMessage"] = productProfile.Name + " Product Profile Saved Successfully";
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
                return View(productProfile);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        
        }

        public IActionResult Delete(int id)
        {
            var productProfile = _productProfileRepo.GetItem(id);
            TempData.Keep();
            return View(productProfile);
        }
        [HttpPost]
        public IActionResult Delete(ProductProfile productProfile)
        {
            try
            {
                productProfile= _productProfileRepo.Delete(productProfile);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(productProfile);

            }


            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];

            TempData["SuccessMessage"] = productProfile.Name + " Deleted Successfully";

            return RedirectToAction(nameof(Index), new { pg = currentPage });

        }
        public IActionResult Details(int id)
        {
            var productProfile = _productProfileRepo.GetItem(id);
            return View(productProfile);
        }
    }
}

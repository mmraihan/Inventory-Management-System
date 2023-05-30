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
    public class ProductGroupController : Controller
    {
        private readonly IProductGroup _productGroupRepo;
        public ProductGroupController(IProductGroup repo)
        {
            _productGroupRepo = repo;
        }

        public IActionResult Index(string sortExpression = "", string searchText="", int pg=1, int pageSize=5)
        {
            var sortModel = new SortModel();
            sortModel.AddColumn("description");
            sortModel.AddColumn("name");
            
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"]=sortModel;
            ViewBag.SearchText= searchText;
            
            var units = _productGroupRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, searchText, pg,pageSize);

            var pager = new PagerModel(units.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;

            TempData["CurrentPage"] = pg;

            return View(units);
        }
        public IActionResult Create()
        {
            var productGroup= new ProductGroup();
            return View(productGroup);
        }
        [HttpPost]
        public IActionResult Create(ProductGroup productGroup)
        {

            bool bolret = false;
            string errMessage = "";
            try
            {
                if (productGroup.Description.Length < 4 || productGroup.Description == null)
                    errMessage = "Product Group Description Must be atleast 4 Characters";

                if (_productGroupRepo.IsItemExists(productGroup.Name) == true)
                    errMessage = errMessage + " " + " Product Group Name " + productGroup.Name + " Exists Already";

                if (errMessage == "")
                {
                    productGroup = _productGroupRepo.Create(productGroup);
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
                return View(productGroup);
            }
            else
            {
                TempData["SuccessMessage"] = productGroup.Name + " Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }                  
        public IActionResult Edit(int id)
        {
            var productGroup = _productGroupRepo.GetItem(id);
            TempData.Keep();
            return View(productGroup);
        }

        [HttpPost]
        public IActionResult Edit(ProductGroup productGroup)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                if (productGroup.Description.Length < 4 || productGroup.Description == null)
                    errMessage = "Product Group Description Must be atleast 4 Characters";

                if (_productGroupRepo.IsItemExists(productGroup.Name, productGroup.Id) == true)
                    errMessage = errMessage + "Product Group Name " + productGroup.Name + " Already Exists";

                if (errMessage == "")
                {
                    productGroup = _productGroupRepo.Edit(productGroup);
                    TempData["SuccessMessage"] = productGroup.Name + " Product Group Saved Successfully";
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
                return View(productGroup);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        
        }

        public IActionResult Delete(int id)
        {
            var productGroup = _productGroupRepo.GetItem(id);
            TempData.Keep();
            return View(productGroup);
        }
        [HttpPost]
        public IActionResult Delete(ProductGroup productGroup)
        {
            try
            {
                productGroup= _productGroupRepo.Delete(productGroup);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(productGroup);

            }


            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];

            TempData["SuccessMessage"] = productGroup.Name + " Deleted Successfully";

            return RedirectToAction(nameof(Index), new { pg = currentPage });

        }
        public IActionResult Details(int id)
        {
            var productGroup = _productGroupRepo.GetItem(id);
            return View(productGroup);
        }
    }
}

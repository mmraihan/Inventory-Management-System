using InventoryManagementSystem.Interfaces;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagementSystem.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProduct _productRepo;
        private readonly IUnit _unitRepo;
        public ProductController(IProduct productRepo, IUnit unitRepo)
        {
            _productRepo = productRepo;
            _unitRepo = unitRepo;
        }

        public IActionResult Index(string sortExpression = "", string searchText="", int pg=1, int pageSize=5)
        {
            var sortModel = new SortModel();
            sortModel.AddColumn("description");
            sortModel.AddColumn("cost");
            sortModel.AddColumn("code");
            sortModel.AddColumn("price");
            sortModel.AddColumn("unit");
            sortModel.AddColumn("name");


            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"]=sortModel;
            ViewBag.SearchText= searchText;
            
            var units = _productRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, searchText, pg,pageSize);

            var pager = new PagerModel(units.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;

            TempData["CurrentPage"] = pg;

            return View(units);
        }
        public IActionResult Create()
        {
            var product= new Product();
            ViewBag.Units= GetUnits();

            return View(product);
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {

            bool bolret = false;
            string errMessage = "";
            try
            {
                if (product.Description.Length < 4 || product.Description == null)
                    errMessage = "Product Description Must be atleast 4 Characters";

                if (_productRepo.IsItemExists(product.Name) == true)
                    errMessage = errMessage + " " + " Product Name " + product.Name + " Exists Already";

                if (errMessage == "")
                {
                    product = _productRepo.Create(product);
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
                return View(product);
            }
            else
            {
                TempData["SuccessMessage"] = product.Name + " Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }                  
        public IActionResult Edit(string id)
        {
            var product = _productRepo.GetItem(id);
            TempData.Keep();
            ViewBag.Units = GetUnits();
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                if (product.Description.Length < 4 || product.Description == null)
                    errMessage = "Product Description Must be atleast 4 Characters";

                if (_productRepo.IsItemExists(product.Name, product.Code) == true)
                    errMessage = errMessage + "Product Name " + product.Name + " Already Exists";

                if (errMessage == "")
                {
                    product = _productRepo.Edit(product);
                    TempData["SuccessMessage"] = product.Name + " Product Saved Successfully";
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
                return View(product);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        
        }

        public IActionResult Delete(string id)
        {
            var product = _productRepo.GetItem(id);
            TempData.Keep();
            return View(product);
        }
        [HttpPost]
        public IActionResult Delete(Product product)
        {
            try
            {
                product= _productRepo.Delete(product);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(product);

            }


            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];

            TempData["SuccessMessage"] = product.Name + " Deleted Successfully";

            return RedirectToAction(nameof(Index), new { pg = currentPage });

        }
        public IActionResult Details(string id)
        {
            var product = _productRepo.GetItem(id);
            return View(product);
        }


        private void PopulateViewbags()
        {

            ViewBag.Units = GetUnits();

        }
        private List<SelectListItem> GetUnits()
        {
            var listOfUnits = new List<SelectListItem>();

            PaginatedList<Unit> units = _unitRepo.GetItems("Name", SortOrder.Ascending, "", 1, 1000);
            listOfUnits = units.Select(ut => new SelectListItem()
            {
                Value = ut.Id.ToString(),
                Text = ut.Name
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Select Unit----"
            };

            listOfUnits.Insert(0, defItem);

            return listOfUnits;
        }

    }
}

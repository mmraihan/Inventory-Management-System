using InventoryManagementSystem.Interfaces;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagementSystem.Controllers
{
    public class PurchaseOrderController : Controller
    {
        private readonly IPurchaseOrder _purchaseOrderRepo;
        private readonly IProduct _productRepo;
        public PurchaseOrderController(IPurchaseOrder purchaseOrderRepo, IProduct productRepo )
        {
            _purchaseOrderRepo = purchaseOrderRepo;
            _productRepo = productRepo;
        }
        public IActionResult Index(string sortExpression = "", string searchText = "", int pg = 1, int pageSize = 5)
        {
            var sortModel = new SortModel();
            sortModel.AddColumn("poumber");
            sortModel.AddColumn("quotationno");

            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = searchText;

            var units = _purchaseOrderRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, searchText, pg, pageSize);

            var pager = new PagerModel(units.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;

            TempData["CurrentPage"] = pg;

            return View(units);
        }

        public IActionResult Create()
        {
            var purchaseOrder = new PurchaseOrderHeader();
            purchaseOrder.PurchaseOrderDetails.Add(new PurchaseOrderDetail() { Id = 1 });

            ViewBag.ProductList = GetPrductList();
            return View(purchaseOrder);
        }
        [HttpPost]
        public IActionResult Create(PurchaseOrderHeader purchaseOrderHeader)
        {

            bool result = false;
            string errMessage = "";
            try
            {
                result = _purchaseOrderRepo.Create(purchaseOrderHeader);
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }

            if (result == false)
            {
                errMessage = errMessage + " " + _purchaseOrderRepo.GetErrors();
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(purchaseOrderHeader);
            }
            else
            {
                TempData["SuccessMessage"] = purchaseOrderHeader.PoNumber + " Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }


        #region Private Method

        private List<SelectListItem> GetPrductList()
        {
            var lstItems = new List<SelectListItem>();

            PaginatedList<Product> items = _productRepo.GetItems("Name", SortOrder.Ascending, "", 1, 1000);
            lstItems = items.Select(ut => new SelectListItem()
            {
                Value = ut.Code,
                Text = ut.Name
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "-Select Product-"
            };

            lstItems.Insert(0, defItem);

            return lstItems;
        }


        #endregion
    }
}

using InventoryManagementSystem.Interfaces;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Tools;
using Microsoft.AspNetCore.Mvc;
using System;

namespace InventoryManagementSystem.Controllers
{
    public class PurchaseOrderController : Controller
    {
        private readonly IPurchaseOrder _purchaseOrderRepo;
        public PurchaseOrderController(IPurchaseOrder purchaseOrderRepo)
        {
            _purchaseOrderRepo = purchaseOrderRepo;
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
            //ViewBag.ExchangeCurrencyId = GetCurrencies();
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
    }
}

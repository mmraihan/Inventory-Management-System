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
        private readonly ISupplier _supplierRepo;
        private readonly ICurrency _currencyRepo;
        public PurchaseOrderController(IPurchaseOrder purchaseOrderRepo, IProduct productRepo, ISupplier supplierRepo, ICurrency currencyRepo)
        {
            _purchaseOrderRepo = purchaseOrderRepo;
            _productRepo = productRepo;
            _supplierRepo = supplierRepo;
            _currencyRepo = currencyRepo;
        }
        public IActionResult Index(string sortExpression = "", string searchText = "", int pg = 1, int pageSize = 5)
        {
            var sortModel = new SortModel();
            sortModel.AddColumn("PoDate");
            sortModel.AddColumn("PoNumber");

            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = searchText;

            var items = _purchaseOrderRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, searchText, pg, pageSize);

            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;

            TempData["CurrentPage"] = pg;

            return View(items);
        }

        public IActionResult Create()
        {
            var purchaseOrder = new PurchaseOrderHeader();
            purchaseOrder.PurchaseOrderDetails.Add(new PurchaseOrderDetail() { Id = 1 });

            ViewBag.ProductList = GetPrductList();
            ViewBag.SupplierList = GetSuppliers();
            ViewBag.PoCurrencyList = GetPoCurrencies();
            ViewBag.BaseCurrencyList = GetBaseCurrencies();
            return View(purchaseOrder);
        }
        [HttpPost]
        public IActionResult Create(PurchaseOrderHeader purchaseOrderHeader)
        {
            purchaseOrderHeader.PurchaseOrderDetails.RemoveAll(a => a.Quantity == 0);
            
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

        public IActionResult Details(int id)
        {
            var item = _purchaseOrderRepo.GetItem(id);

            ViewBag.ProductList = GetPrductList();
            ViewBag.SupplierList = GetSuppliers();
            ViewBag.PoCurrencyList = GetPoCurrencies();
            ViewBag.BaseCurrencyList = GetBaseCurrencies();

            return View(item);
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
                Text = "--Select--"
            };

            lstItems.Insert(0, defItem);

            return lstItems;
        }

        private List<SelectListItem> GetSuppliers()
        {
            var lstSuppliers = new List<SelectListItem>();

            PaginatedList<Supplier> suppliers = _supplierRepo.GetItems("Name", SortOrder.Ascending, "", 1, 1000);

            lstSuppliers = suppliers.Select(sp => new SelectListItem()
            {
                Value = sp.Id.ToString(),
                Text = sp.Name
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "--- Select Supplier ---"
            };

            lstSuppliers.Insert(0, defItem);

            return lstSuppliers;
        }

        private List<SelectListItem> GetPoCurrencies()
        {
            var lstCurrencies = new List<SelectListItem>();

            PaginatedList<Currency> currencies = _currencyRepo.GetItems("Name", SortOrder.Ascending, "", 1, 1000);

            lstCurrencies = currencies.Select(sp => new SelectListItem()
            {
                Value = sp.Id.ToString(),
                Text = sp.Name
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "--- Select PO Currency ---"
            };

            lstCurrencies.Insert(0, defItem);

            return lstCurrencies;
        }

        private List<SelectListItem> GetBaseCurrencies()
        {
            var lstCurrencies = new List<SelectListItem>();

            PaginatedList<Currency> currencies = _currencyRepo.GetItems("Name", SortOrder.Ascending, "BDT", 1, 1000);
           
            lstCurrencies = currencies.Select(sp => new SelectListItem()
            {
                Value = sp.Id.ToString(),
                Text = sp.Name
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "--- Select Base Currency ---"
            };

            lstCurrencies.Insert(0, defItem);

            return lstCurrencies;
        }



        #endregion
    }
}

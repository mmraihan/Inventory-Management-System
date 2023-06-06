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
    public class CurrencyController : Controller
    {
        private readonly ICurrency _currencyRepo;

        public CurrencyController(ICurrency currencyRepo)
        {
            _currencyRepo = currencyRepo;
        }

        public IActionResult Index(string sortExpression = "", string searchText="", int pg=1, int pageSize=5)
        {
            var sortModel = new SortModel();
            sortModel.AddColumn("description");
            sortModel.AddColumn("name");
            
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"]=sortModel;
            ViewBag.SearchText= searchText;
            
            var currencys = _currencyRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, searchText, pg,pageSize);

            var pager = new PagerModel(currencys.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;

            TempData["CurrentPage"] = pg;

            return View(currencys);
        }
        public IActionResult Create()
        {
            var currency= new Currency();
            ViewBag.ExchangeCurrencyId = GetCurrencies();
            return View(currency);
        }
        [HttpPost]
        public IActionResult Create(Currency currency)
        {

            bool bolret = false;
            string errMessage = "";
            try
            {
                bolret = _currencyRepo.Create(currency);
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }

            if (bolret == false)
            {
                errMessage = errMessage + " " + _currencyRepo.GetErrors();
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(currency);
            }
            else
            {
                TempData["SuccessMessage"] = currency.Name + " Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult Edit(int id)
        {
            var currency = _currencyRepo.GetItem(id);
            TempData.Keep();
            ViewBag.ExchangeCurrencyId = GetCurrencies();
            return View(currency);
        }

        [HttpPost]
        public IActionResult Edit(Currency currency)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                bolret = _currencyRepo.Edit(currency);
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
                errMessage = errMessage + " " + _currencyRepo.GetErrors();
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(currency);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        
        }

        public IActionResult Delete(int id)
        {
            var currency = _currencyRepo.GetItem(id);
            TempData.Keep();
            ViewBag.ExchangeCurrencyId = GetCurrencies();
            return View(currency);
        }
        [HttpPost]
        public IActionResult Delete(Currency currency)
        {
            bool bolret = false;
            string errMessage = "";
      
            try
            {
                bolret= _currencyRepo.Delete(currency);
            }

            catch (Exception ex)
            {
                errMessage = ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(currency);

            }


            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];

           
            if (bolret == false)
            {
                errMessage = errMessage + " " + _currencyRepo.GetErrors();
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(currency);
            }
            else
            {
                TempData["SuccessMessage"] = currency.Name + " Deleted Successfully";
                return RedirectToAction(nameof(Index), new { pg = currentPage });
            }

           

        }
        public IActionResult Details(int id)
        {
            var currency = _currencyRepo.GetItem(id);
            ViewBag.ExchangeCurrencyId = GetCurrencies();
            return View(currency);
        }

        #region Private Method
        private List<SelectListItem> GetCurrencies()
        {
            var listOfUnits = new List<SelectListItem>();

            PaginatedList<Currency> currencies = _currencyRepo.GetItems("Name", SortOrder.Ascending, "", 1, 1000);
            listOfUnits = currencies.Select(ut => new SelectListItem()
            {
                Value = ut.Id.ToString(),
                Text = ut.Name
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Select Currency----"
            };

            listOfUnits.Insert(0, defItem);

            return listOfUnits;
        }
        #endregion


    }
}

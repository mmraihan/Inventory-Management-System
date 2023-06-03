using InventoryManagementSystem.Data;
using InventoryManagementSystem.Interfaces;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagementSystem.Repositories
{
    public class CurrencyRepositry : ICurrency
    {
        private readonly InventoryContext _context;

        private string _errors = "";
        public CurrencyRepositry(InventoryContext context)
        {
            _context = context;
        }
        public bool Create(Currency currency)
        {
            try
            {
                if (!IsDescriptionValid(currency))
                {
                    return false;
                }

                if (IsItemExists(currency.Name))
                {
                    return false;
                }                  
                _context.Currencies.Add(currency);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _errors= "SQL exception occured, Error Info: " + ex.Message;
                return false; 
            }
        }

        public bool Delete(Currency currency)
        {
            try
            {
                _context.Currencies.Attach(currency);
                _context.Entry(currency).State = EntityState.Deleted;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _errors = "SQL Exception Occured, Error Info: " + ex.Message;
                return false;
            }
        }

        public bool Edit(Currency currency)
        {
            try
            {
                // Check validation against Rule -1
                if (!IsDescriptionValid(currency))
                {
                    return false;
                }
                if (IsItemExists(currency.Name, currency.Id))
                {
                    return false;
                }

                _context.Currencies.Attach(currency);
                _context.Entry(currency).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _errors = "SQL Exception Occured, Error Info: " + ex.Message;
                return false;
            }
        }

        public string GetErrors()
        {
            return _errors;
        }

        public Currency GetItem(int id)
        {
            var item = _context.Currencies.Where(u => u.Id == id).FirstOrDefault();
            return item;
        }

        public PaginatedList<Currency> GetItems(string SortProperty, SortOrder sortOrder, string searchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<Currency> items;

            if (searchText != "" && searchText != null)
            {
                items = _context.Currencies.Where(n => n.Name.Contains(searchText) || n.Description.Contains(searchText))
                    .ToList();
            }
            else
                items = _context.Currencies.ToList();

            items = DoSort(items, SortProperty, sortOrder);

            PaginatedList<Currency> retItems = new PaginatedList<Currency>(items, pageIndex, pageSize);

            return retItems;
        }

        public bool IsCurrencyCombExists(int srcCurrencyId, int excCurrencyId)
        {
            int ct = _context.Currencies.Where(n => n.Id == srcCurrencyId && n.ExchangeCurrencyId== excCurrencyId).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsItemExists(string name)
        {
            int ct = _context.Currencies.Where(n => n.Name.ToLower() == name.ToLower()).Count();
            if (ct > 0)
            {
                _errors = name + " Exists Already";
                return true;
            }             
            else
                return false;
        }

      
        public bool IsItemExists(string name, int id)
        {
            int ct = _context.Currencies.Where(n => n.Name.ToLower() == name.ToLower() && n.Id != id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }


        #region Private Methods

        private List<Currency> DoSort(List<Currency> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "name")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.Name).ToList();
                else
                    items = items.OrderByDescending(n => n.Name).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(d => d.Description).ToList();
                else
                    items = items.OrderByDescending(d => d.Description).ToList();
            }

            return items;
        }
        
        //Rule -1
        private bool IsDescriptionValid(Currency currency) 
        {
            if (currency.Description.Length < 4 || currency.Description == null)
            {
                _errors = "Currency Description Must be atleast 4 Characters";
                return false;
            }
            return true;

        }


        #endregion
    }
}

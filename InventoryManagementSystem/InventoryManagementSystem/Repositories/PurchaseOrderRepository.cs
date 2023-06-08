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
    public class PurchaseOrderRepository : IPurchaseOrder
    {
        private readonly InventoryContext _context;

        public string _errors = "";
        public PurchaseOrderRepository(InventoryContext context)
        {
            _context = context;
        }

        public bool Create(PurchaseOrderHeader purchaseOrderHeader)
        {
            bool result = false;
            _errors = "";
            try
            {
                _context.PurchaseOrderHeaders.Add(purchaseOrderHeader);
                _context.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {
                _errors = "SQL exception occured, Error Info: " + ex.Message;
                
            }
            return result;
        }

        public bool Delete(PurchaseOrderHeader purchaseOrderHeader)
        {
            //bool result = false;
            //_errors = "";
            //try
            //{
            //    _context.PurchaseOrderHeaders.Attach(purchaseOrderHeader);
            //    _context.Entry(purchaseOrderHeader).State = EntityState.Deleted;
            //    _context.SaveChanges();
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    _errors = "SQL Exception Occured, Error Info: " + ex.Message;

            //}
            //return result;
            return false;
        }

        public bool Edit(PurchaseOrderHeader purchaseOrderHeader)
        {
            return false;
        }

        public PurchaseOrderHeader GetItem(int id)
        {
            var item = _context.PurchaseOrderHeaders.Where(u => u.Id == id)
                .Include(p=>p.PurchaseOrderDetails)
                .FirstOrDefault();
            return item;
        }

        public PaginatedList<PurchaseOrderHeader> GetItems(string SortProperty, SortOrder sortOrder, string searchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<PurchaseOrderHeader> items;

            if (searchText != "" && searchText != null)
            {
                items = _context.PurchaseOrderHeaders.Where(n => n.PoNumber.Contains(searchText) || n.QuotationNo.Contains(searchText))
                    .Include(p=>p.Supplier)
                    .Include(p=>p.BaseCurrency)
                    .Include(p=>p.POCurrency)
                    .ToList();
            }
            else
                items = _context.PurchaseOrderHeaders
                    .Include(p => p.Supplier)
                    .Include(p => p.BaseCurrency)
                    .Include(p => p.POCurrency)
                    .ToList();

            items = DoSort(items, SortProperty, sortOrder);

            PaginatedList<PurchaseOrderHeader> retItems = new PaginatedList<PurchaseOrderHeader>(items, pageIndex, pageSize);

            return retItems;
        }

        public bool IsPoNumberExists(string poNumber)
        {
            var item = _context.PurchaseOrderHeaders.Where(n => n.PoNumber.ToLower()==poNumber.ToLower()).Count();
            if (item>0)
            {
                return true;
            }         
                return false;
           
        }

        public bool IsPoNumberExists(string poNumber, int id=0)
        {
            if (id == 0)
                return IsPoNumberExists(poNumber);

            var item = _context.PurchaseOrderHeaders.Where(n => n.PoNumber == poNumber).Max(cd => cd.Id);
            if (item == 0 || item == id)
                return false;
            else
                return true;

            //var item = _context.PurchaseOrderHeaders.Where(n => n.PoNumber.ToLower() == poNumber.ToLower()
            //    && n.Id==id ).Count();
            //if (item>0)
            //{
            //    return true;
            //}
            //return false;
        }

        public bool IsQuotationNoExists(string quotationNo)
        {
            var item = _context.PurchaseOrderHeaders.Where(n => n.QuotationNo.ToLower() == quotationNo.ToLower()).Count();
            if (item > 0)
            {
                return true;
            }
            return false;
        }

        public bool IsQuotationNoExists(string quotationNo, int id)
        {
            if (id == 0)
                return IsPoNumberExists(quotationNo);

            var item = _context.PurchaseOrderHeaders.Where(n => n.QuotationNo == quotationNo).Max(cd => cd.Id);
            if (item == 0 || item == id)
                return false;
            else
                return true;
        }

        public string GetErrors()
        {
            return _errors;
        }


        #region Private Methods


        private List<PurchaseOrderHeader> DoSort(List<PurchaseOrderHeader> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "ponumber")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.PoNumber).ToList();
                else
                    items = items.OrderByDescending(n => n.PoNumber).ToList();
            }
            else if(SortProperty.ToLower() == "podate")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(d => d.PoDate).ToList();
                else
                    items = items.OrderByDescending(d => d.PoDate).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(d => d.PoDate).ToList();
                else
                    items = items.OrderByDescending(d => d.PoDate).ToList();
            }

            return items;
        }


        #endregion
    }
}

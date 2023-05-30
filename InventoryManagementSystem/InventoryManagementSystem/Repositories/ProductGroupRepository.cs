using InventoryManagementSystem.Data;
using InventoryManagementSystem.Interfaces;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Tools;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagementSystem.Repositories
{
    public class ProductGroupRepository : IProductGroup
    {
        private readonly InventoryContext _context;
        public ProductGroupRepository(InventoryContext context)
        {
            _context = context;
        }

        public ProductGroup Create(ProductGroup productGroup)
        {
            _context.ProductGroups.Add(productGroup);
            _context.SaveChanges();
            return productGroup;
        }

        public ProductGroup Delete(ProductGroup productGroup)
        {
            _context.ProductGroups.Attach(productGroup);
            _context.Entry(productGroup).State = EntityState.Deleted;
            _context.SaveChanges();
            return productGroup;
        }

        public ProductGroup Edit(ProductGroup productGroup)
        {
            _context.ProductGroups.Attach(productGroup);
            _context.Entry(productGroup).State = EntityState.Modified;
            _context.SaveChanges();
            return productGroup;
        }


        private List<ProductGroup> DoSort(List<ProductGroup> items, string SortProperty, SortOrder sortOrder)
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

        public PaginatedList<ProductGroup> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<ProductGroup> items;

            if (SearchText != "" && SearchText != null)
            {
                items = _context.ProductGroups.Where(n => n.Name.Contains(SearchText) || n.Description.Contains(SearchText))
                    .ToList();
            }
            else
                items = _context.ProductGroups.ToList();

            items = DoSort(items, SortProperty, sortOrder);

            PaginatedList<ProductGroup> retItems = new PaginatedList<ProductGroup>(items, pageIndex, pageSize);

            return retItems;
        }

        public ProductGroup GetItem(int id)
        {
            ProductGroup item = _context.ProductGroups.Where(u => u.Id == id).FirstOrDefault();
            return item;
        }
        public bool IsItemExists(string name)
        {
            int ct = _context.ProductGroups.Where(n => n.Name.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsItemExists(string name, int Id)
        {
            int ct = _context.ProductGroups.Where(n => n.Name.ToLower() == name.ToLower() && n.Id != Id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

    }
}

using InventoryManagementSystem.Data;
using InventoryManagementSystem.Interfaces;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Tools;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagementSystem.Repositories
{
    public class ProductProfileRepository : IProductProfile
    {
        private readonly InventoryContext _context;
        public ProductProfileRepository(InventoryContext context)
        {
            _context = context;
        }

        public ProductProfile Create(ProductProfile productProfile)
        {
            _context.ProductProfiles.Add(productProfile);
            _context.SaveChanges();
            return productProfile;
        }

        public ProductProfile Delete(ProductProfile productProfile)
        {
            _context.ProductProfiles.Attach(productProfile);
            _context.Entry(productProfile).State = EntityState.Deleted;
            _context.SaveChanges();
            return productProfile;
        }

        public ProductProfile Edit(ProductProfile productProfile)
        {
            _context.ProductProfiles.Attach(productProfile);
            _context.Entry(productProfile).State = EntityState.Modified;
            _context.SaveChanges();
            return productProfile;
        }


        private List<ProductProfile> DoSort(List<ProductProfile> items, string SortProperty, SortOrder sortOrder)
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

        public PaginatedList<ProductProfile> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<ProductProfile> items;

            if (SearchText != "" && SearchText != null)
            {
                items = _context.ProductProfiles.Where(n => n.Name.Contains(SearchText) || n.Description.Contains(SearchText))
                    .ToList();
            }
            else
                items = _context.ProductProfiles.ToList();

            items = DoSort(items, SortProperty, sortOrder);

            PaginatedList<ProductProfile> retItems = new PaginatedList<ProductProfile>(items, pageIndex, pageSize);

            return retItems;
        }

        public ProductProfile GetItem(int id)
        {
            ProductProfile item = _context.ProductProfiles.Where(u => u.Id == id).FirstOrDefault();
            return item;
        }
        public bool IsItemExists(string name)
        {
            int ct = _context.ProductProfiles.Where(n => n.Name.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsItemExists(string name, int Id)
        {
            int ct = _context.ProductProfiles.Where(n => n.Name.ToLower() == name.ToLower() && n.Id != Id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

    }
}

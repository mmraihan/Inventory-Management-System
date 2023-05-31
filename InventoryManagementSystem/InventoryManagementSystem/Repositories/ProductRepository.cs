using InventoryManagementSystem.Data;
using InventoryManagementSystem.Interfaces;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Tools;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagementSystem.Repositories
{
    public class ProductRepository : IProduct
    {
        private readonly InventoryContext _context;
        public ProductRepository(InventoryContext context)
        {
            _context = context;
        }

        public Product Create(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }

        public Product Delete(Product product)
        {
            product = PriveteGetItem(product.Code);
            _context.Products.Attach(product);
            _context.Entry(product).State = EntityState.Deleted;
            _context.SaveChanges();
            return product;
        }

        public Product Edit(Product product)
        {
            _context.Products.Attach(product);
            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();
            return product;
        }


        private List<Product> DoSort(List<Product> items, string SortProperty, SortOrder sortOrder)
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

        public PaginatedList<Product> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<Product> items;

            if (SearchText != "" && SearchText != null)
            {
                items = _context.Products.Where(n => n.Name.Contains(SearchText) || n.Description.Contains(SearchText))
                    .Include(c=>c.Units)
                    .ToList();
            }
            else
                items = _context.Products
                    .Include(c => c.Units)
                    .Include(c=>c.Brands)
                    .Include(c=>c.Categories)
                    .Include(c=>c.ProductGroups)
                    .Include(c=>c.ProductProfiles)
                    .ToList();

            items = DoSort(items, SortProperty, sortOrder);

            PaginatedList<Product> retItems = new PaginatedList<Product>(items, pageIndex, pageSize);

            return retItems;
        }

        public Product GetItem(string code)
        {
            Product item = _context.Products.Where(u => u.Code == code)
                .Include(c=>c.Units)
                .FirstOrDefault();

            item.BreifPhotoName = GetBriefPhotoName(item.PhotoUrl);
            return item;
        }
        public bool IsItemExists(string name)
        {
            int ct = _context.Products.Where(n => n.Name.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsItemExists(string name, string Code)
        {
            if (Code == "")
                return IsItemExists(name);
            var strCode = _context.Products.Where(n => n.Name == name).Max(cd => cd.Code);
            if (strCode == null || strCode == Code)
                return false;
            else
                return true;
        }
        public bool IsItemCodeExists(string itemCode)
        {
            int ct = _context.Products.Where(n => n.Code.ToLower() == itemCode.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsItemCodeExists(string itemCode, string name)
        {
            if (name == "")
                return IsItemCodeExists(itemCode);
            var strName = _context.Products.Where(n => n.Code == itemCode).Max(nm => nm.Name);
            if (strName == null || strName == name)
                return false;
            else
                return IsItemExists(name);
        }

        #region Private Method

        private Product PriveteGetItem(string code)
        {
            Product item = _context.Products.Where(u => u.Code == code).FirstOrDefault();
            return item;
        }

        private string GetBriefPhotoName(string fileName)
        {
            if (fileName == null)
                return "";

            string[] words = fileName.Split('_');
            return words[words.Length - 1];
        }

        #endregion

    }
}

using InventoryManagementSystem.Models;
using InventoryManagementSystem.Tools;

namespace InventoryManagementSystem.Interfaces
{
    public interface IProduct
    {
        PaginatedList<Product> GetItems(string SortProperty, SortOrder sortOrder, string searchText = "", int pageIndex = 1, int pageSize = 5);
        Product GetItem(string code);
        Product Create(Product product);

        Product Edit(Product product);

        Product Delete(Product product);
        bool IsItemExists(string name);
        bool IsItemExists(string name, string Code);
    }
}

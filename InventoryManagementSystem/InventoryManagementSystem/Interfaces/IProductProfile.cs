using InventoryManagementSystem.Models;
using InventoryManagementSystem.Tools;

namespace InventoryManagementSystem.Interfaces
{
    public interface IProductProfile
    {
        PaginatedList<ProductProfile> GetItems(string SortProperty, SortOrder sortOrder, string searchText = "", int pageIndex = 1, int pageSize = 5);
        ProductProfile GetItem(int id);
        ProductProfile Create(ProductProfile category);

        ProductProfile Edit(ProductProfile category);

        ProductProfile Delete(ProductProfile category);
        bool IsItemExists(string name);
        bool IsItemExists(string name, int id);
    }
}

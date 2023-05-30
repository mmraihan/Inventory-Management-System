using InventoryManagementSystem.Models;
using InventoryManagementSystem.Tools;

namespace InventoryManagementSystem.Interfaces
{
    public interface IProductGroup
    {
        PaginatedList<ProductGroup> GetItems(string SortProperty, SortOrder sortOrder, string searchText = "", int pageIndex = 1, int pageSize = 5);
        ProductGroup GetItem(int id);
        ProductGroup Create(ProductGroup category);

        ProductGroup Edit(ProductGroup category);

        ProductGroup Delete(ProductGroup category);
        bool IsItemExists(string name);
        bool IsItemExists(string name, int id);
    }
}

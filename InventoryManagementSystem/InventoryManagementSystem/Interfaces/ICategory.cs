using InventoryManagementSystem.Models;
using InventoryManagementSystem.Tools;

namespace InventoryManagementSystem.Interfaces
{
    public interface ICategory
    {
        PaginatedList<Category> GetItems(string SortProperty, SortOrder sortOrder, string searchText = "", int pageIndex = 1, int pageSize = 5);
        Category GetItem(int id);
        Category Create(Category category);

        Category Edit(Category category);

        Category Delete(Category category);
        bool IsItemExists(string name);
        bool IsItemExists(string name, int id);
    }
}

using InventoryManagementSystem.Models;
using InventoryManagementSystem.Tools;

namespace InventoryManagementSystem.Interfaces
{
    public interface IBrand
    {
        PaginatedList<Brand> GetItems(string SortProperty, SortOrder sortOrder, string searchText = "", int pageIndex = 1, int pageSize = 5);
        Brand GetItem(int id);
        Brand Create(Brand category);

        Brand Edit(Brand category);

        Brand Delete(Brand category);
        bool IsItemExists(string name);
        bool IsItemExists(string name, int id);
    }
}

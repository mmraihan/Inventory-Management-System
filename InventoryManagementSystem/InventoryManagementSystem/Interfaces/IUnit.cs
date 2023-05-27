using InventoryManagementSystem.Models;
using InventoryManagementSystem.Tools;
using System.Collections.Generic;

namespace InventoryManagementSystem.Interfaces
{
    public interface IUnit
    {
        PaginatedList<Unit> GetItems(string SortProperty, SortOrder sortOrder, string searchText = "", int pageIndex = 1, int pageSize = 5);
        Unit GetUnit(int id); 
        Unit Create(Unit unit);

        Unit Edit(Unit unit);

        Unit Delete(Unit unit);
        bool IsUnitNameExists(string name);
        bool IsUnitNameExists(string name, int id);

    }
}

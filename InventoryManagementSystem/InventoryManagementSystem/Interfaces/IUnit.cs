using InventoryManagementSystem.Models;
using InventoryManagementSystem.Tools;
using System.Collections.Generic;

namespace InventoryManagementSystem.Interfaces
{
    public interface IUnit
    {
        List<Unit> GetItems(string SortProperty, SortOrder sortOrder);
        Unit GetUnit(int id); 
        Unit Create(Unit unit);

        Unit Edit(Unit unit);

        Unit Delete(Unit unit);

    }
}

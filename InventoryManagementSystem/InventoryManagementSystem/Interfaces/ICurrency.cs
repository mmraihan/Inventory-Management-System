using InventoryManagementSystem.Models;
using InventoryManagementSystem.Tools;

namespace InventoryManagementSystem.Interfaces
{
    public interface ICurrency
    {
        PaginatedList<Currency> GetItems(string SortProperty, SortOrder sortOrder, string searchText = "", int pageIndex = 1, int pageSize = 5);
        Currency GetItem(int id);
        bool Create(Currency currency);
        bool Edit(Currency currency);
        bool Delete(Currency currency);
        bool IsItemExists(string name);
        bool IsItemExists(string name, int id);
        bool IsCurrencyCombExists(int srcCurrencyId, int excCurrencyId);
        string GetErrors();
    }
}

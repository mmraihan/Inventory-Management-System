using InventoryManagementSystem.Models;
using InventoryManagementSystem.Tools;

namespace InventoryManagementSystem.Interfaces
{
    public interface IPurchaseOrder
    {
        PaginatedList<PurchaseOrderHeader> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5); //read all
        PurchaseOrderHeader GetItem(int id); // read particular item

        bool Create(PurchaseOrderHeader supplier);
        bool Edit(PurchaseOrderHeader supplier);
        bool Delete(PurchaseOrderHeader supplier);
        bool IsPoNumberExists(string poNumber);
        bool IsPoNumberExists(string poNumber, int id);

        bool IsQuotationNoExists(string quotationNo);
        bool IsQuotationNoExists(string quotationNo, int id);
        string GetErrors();



    }
}

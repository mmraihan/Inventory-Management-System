using System.Collections.Generic;
using System.Linq;

namespace InventoryManagementSystem.Tools
{
    public enum SortOrder { Ascending = 0, Descending = 1 }
    public class SortModel
    {
        private string UpIcon = "fa fa-arrow-up";
        private string DownIcon = "fa fa-arrow-down";
        private List<SortableColumn> sortableColumns = new List<SortableColumn>();

        public SortOrder SortedOrder { get; set; }
        public string SortedProperty { get; set; }



        public void AddColumn(string colName, bool isDefaultColumn = false)
        {
            SortableColumn sortableColumn = this.sortableColumns.Where(c => c.ColumnName.ToLower() == colName.ToLower()).SingleOrDefault();

            if (sortableColumn == null)
            {
                sortableColumns.Add(new SortableColumn() { ColumnName = colName });
            }

            if (isDefaultColumn = true || sortableColumns.Count == 1)
            {
                SortedProperty = colName;
                SortedOrder = SortOrder.Ascending;
            }
        }

        public SortableColumn GetColumn(string colName)
        {
            SortableColumn sortableColumn = this.sortableColumns.Where(c => c.ColumnName.ToLower() == colName.ToLower()).SingleOrDefault();

            if (sortableColumn == null)
            {
                sortableColumns.Add(new SortableColumn() { ColumnName = colName });
            }
            return sortableColumn;
        }

        public void ApplySort(string sortExpression)
        {

            if (sortExpression == "")
            {
                sortExpression = this.SortedProperty;
            }

            sortExpression = sortExpression.ToLower();

            foreach (var item in this.sortableColumns)
            {
                item.SortIcon = "";
                item.SortExpression = item.ColumnName;

                if (sortExpression == item.ColumnName.ToLower())
                {
                    this.SortedOrder = SortOrder.Ascending;
                    this.SortedProperty = item.ColumnName;
                    item.SortIcon = DownIcon;
                    item.SortExpression = item.ColumnName + "_desc";
                }

                if (sortExpression == item.ColumnName.ToLower() + "_desc")
                {
                    this.SortedOrder = SortOrder.Descending;
                    this.SortedProperty = item.ColumnName;
                    item.SortIcon = UpIcon;
                    item.SortExpression = item.ColumnName;

                }
            }

        }

    }


    public class SortableColumn
    {
        public string ColumnName { get; set; }
        public string SortExpression { get; set; }
        public string SortIcon { get; set; }
    }

}
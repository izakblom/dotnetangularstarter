using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Utilities.Datatable
{
    public class DataTableFilter
    {
        //public List<ColumnObj> columns { get; set; }
        //public List<OrderObj> orderbyColumns { get; set; }

        /// <summary>
        /// Amount of records to skip
        /// </summary>
        public int skip { get; set; }

        /// <summary>
        /// Amount of records to load
        /// </summary>
        public int take { get; set; }

        /// <summary>
        /// The name of the column by which data should be sorted
        /// </summary>
        public string sortColumnName { get; set; }

        /// <summary>
        /// Column sort direction - either 'asc' or 'desc'
        /// </summary>
        public string sortDirection { get; set; }


        public DataTableFilter()
        {
            //columns = new List<ColumnObj>();
            //orderbyColumns = new List<OrderObj>();
        }
    }

    public class ColumnObj
    {
        public string data { get; set; }
        public string name { get; set; }
        public bool searchable { get; set; }
        public bool orderable { get; set; }

        //public SearchObj search { get; set; }

        public ColumnObj()
        {
            //search = new SearchObj();
        }
    }

    public class OrderObj
    {
        public int column { get; set; }
        public string dir { get; set; }
    }
}

using System.Collections.Generic;

namespace Common.DataObjects
{
    public class DTODataTableResult<T>
    {
        public int totalRecordCount { get; set; } = 0;
        public int filteredRecordCount { get; set; } = 0;

        /// <summary>
        /// Contains the data to be displayed in the data table
        /// </summary>
        public IEnumerable<T> data { get; set; }
        /// <summary>
        /// Contains column definitions to be used to setup datatable columns
        /// </summary>
        public List<DTODataTableColumnDefinition> columns;

        public DTODataTableResult()
        {
            data = new List<T>();
            columns = new List<DTODataTableColumnDefinition>();
        }
    }

    public class DTODataTableColumnDefinition
    {
        /// <summary>
        /// To be displayed as column header
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// This should correspond to the property name in the class of each data element represented in this column
        /// </summary>
        public string propertyName { get; set; }

        /// <summary>
        /// Whether the column should be orderable or not
        /// </summary>
        public bool orderable { get; set; }

        /// <summary>
        /// Whether the column should be made visible
        /// </summary>
        public bool visible { get; set; }

        /// <summary>
        /// Whether the column should be filterable by text
        /// </summary>
        public bool searchable { get; set; }

        /// <summary>
        /// The URL from which search options should be retrieved for display on the frontend
        /// </summary>
        public string searchOptionsURL { get; set; }

        /// <summary>
        /// Specify control type for special filter type. Current options are: "datepicker"
        /// </summary>
        public string specialFilterControlType { get; set; }

        public string[] searchOptions { get; set; }

    }
}

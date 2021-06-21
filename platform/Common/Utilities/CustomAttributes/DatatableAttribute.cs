using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Utilities.CustomAttributes
{
    public class DatatableAttribute : Attribute
    {

        public string Title { get; private set; }
        public string DatabasePath { get; private set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public bool Visible { get; set; }
        public string SearchOptionsUrl { get; set; }
        public string SpecialFilterControlType { get; set; }
        public string[] SearchOptions { get; set; }

        /// <summary>
        ///         
        /// </summary>
        /// <param name="title">This translates to the column headings of the frontend datatable</param>
        /// <param name="databasePath">This should be the table.propertyname of the database entity from which the data for the column should be loaded</param>
        /// <param name="searchable">Should the data in the column be searchable?</param>
        /// <param name="orderable">Should the data in the column be orderable?</param>
        /// <param name="visible">Should the data in the column be visible?</param>
        /// <param name="searchOptionsUrl">Url postfix (eg. api/admin/options) from which search options should be retrieved, if multiple options</param>
        /// <param name="specialFilterControlType">Specify control type for special filter type. Current options are: "datepicker"</param>
        public DatatableAttribute(
            string title,
            string databasePath,
            bool searchable = false,
            bool orderable = false,
            bool visible = true,
            string searchOptionsUrl = "",
            string specialFilterControlType = "",
            string[] searchOptions = null)
        {
            this.Title = title;
            this.DatabasePath = databasePath;
            this.Searchable = searchable;
            this.Orderable = orderable;
            this.Visible = visible;
            this.SearchOptionsUrl = searchOptionsUrl;
            this.SpecialFilterControlType = specialFilterControlType;
            this.SearchOptions = searchOptions;
        }
    }

    public static class DatatableAttributeHelper
    {

        public static DTODatatablesStructure BuildFromType(Type type)
        {

            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

            var res = new DTODatatablesStructure();
            res.table = type.Name;

            //Build the column definitions from the custom attribute annotations

            res.columnDefinitions = new List<DTODatatablesColumnDefinition>();

            props.ToList().ForEach(p =>
            {

                var attrs = p.GetCustomAttributes(false).ToList();

                if (attrs.Count > 0)
                {
                    attrs.ForEach(f =>
                    {
                        if (f is DatatableAttribute)
                        {
                            var col = new DTODatatablesColumnDefinition();
                            var dt = ((DatatableAttribute)f);

                            col.data = p.Name;
                            col.dbName = dt.DatabasePath == null ? "" : dt.DatabasePath;
                            col.orderable = dt.Orderable;
                            col.searchable = dt.Searchable;
                            col.title = dt.Title;
                            col.visible = dt.Visible;
                            col.searchOptionsUrl = dt.SearchOptionsUrl;
                            col.propertyName = Char.ToLowerInvariant(p.Name[0]) + p.Name.Substring(1);
                            col.specialFilterControlType = dt.SpecialFilterControlType;
                            col.searchOptions = dt.SearchOptions;

                            res.columnDefinitions.Add(col);
                        }


                    });



                }

            });



            return res;
        }

    }

    public class DTODatatablesStructure
    {

        public string table { get; set; }
        public List<DTODatatablesColumnDefinition> columnDefinitions { get; set; }

        public DTODatatablesStructure()
        {

            columnDefinitions = new List<DTODatatablesColumnDefinition>();
        }
    }

    public class DTODatatablesColumnDefinition
    {
        public string data { get; set; }
        public string dbName { get; set; }
        public string title { get; set; }
        public bool searchable { get; set; } = false;
        public bool orderable { get; set; } = true;
        public int index { get; set; }
        public bool visible { get; set; } = true;
        public string propertyName { get; set; }
        public string searchOptionsUrl { get; set; }
        public string specialFilterControlType { get; set; }
        public string[] searchOptions { get; set; }
    }
}

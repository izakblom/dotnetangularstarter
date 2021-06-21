using Common.Utilities.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Utilities.Datatable
{
    public class DatatableQuery
    {
        protected string GetFilterClause<T>(T filter, DTODatatablesStructure queryStructure)
        {
            string clause = "";
            if (filter != null)
            {
                var filterStrings = new List<string>();

                /*
                 *  The Filter Object's properties should be named according to the returned DTO result's column definitions list's propertyName properties
                 *  Use reflection to find the matching columnDefinition object, which should contain the correct databaseName/path from which the filter querystring can
                 *  be constructed. This way, we minimize the occurrences of hard-coded database paths
                 */
                var properties = typeof(T).GetProperties();
                foreach (var prop in properties)
                {
                    if (prop.GetValue(filter) != null && !String.IsNullOrEmpty(prop.GetValue(filter).ToString()))
                    {
                        var matchingColProperty = queryStructure.columnDefinitions.Find(dtcol => dtcol.data.Equals(prop.Name));
                        if (matchingColProperty != null)
                            filterStrings.Add($"({matchingColProperty.dbName} LIKE '%{prop.GetValue(filter).ToString()}%') ");
                    }
                }

                filterStrings.ForEach(f =>
                {

                    if (filterStrings.IndexOf(f) == 0)
                        clause += $" WHERE {f} ";
                    else
                        clause += $" AND {f} ";

                });
            }
            return clause;
        }
    }
}

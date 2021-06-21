

namespace Common.Utilities.Datatable
{
    public class DTOFilter<T> : DataTableFilter
    {
        public T filter { get; set; }

        public DTOFilter() : base()
        {

        }
    }
}

using System.Collections.Generic;

namespace Common.Files.Xls
{
    public interface IXlsSheet
    {
        string name { get; set; }
        int indexOfHeader { get; set; }
        int indexOfData { get; set; }
        List<IRow> rows { get; set; }
    }
}
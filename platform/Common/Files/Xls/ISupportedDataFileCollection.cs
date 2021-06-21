using System.Collections.Generic;

namespace Common.Files.Xls
{
    public interface ISupportedDataFileCollection
    {
        ICollection<IXlsFile> GetXlsFiles();
        //ICollection<ICsvFile> GetCsvFiles();
    }
}

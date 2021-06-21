using System.Collections.Generic;

namespace Common.Files.Xls
{
    public interface IXlsFile
    {
        List<IXlsSheet> sheets { get; set; }
        int TemplateTypeId { get; }
    }
}

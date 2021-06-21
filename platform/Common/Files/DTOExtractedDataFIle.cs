using System;

namespace Common.Files
{
    public class DTOExtractedDataFIle
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public int ExtractedFileDataStatusId { get; set; }
        public string Content { get; set; }
        public int PackageNumber { get; set; }
        public int TotalPackages { get; set; }
        public string FileId { get; set; }
        public string FileClassType { get; set; }
    }
}
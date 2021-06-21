using Common.PubSub;
using System;

namespace Common.DataObjects.PubSub
{
    public class DTOExtractedFile : Message
    {
        public DateTime extractedOn { get; set; }
        public string serializedContent { get; set; }
        public string templateClassName { get; set; }
        public string sheetClassName { get; set; }
        public string rowClassName { get; set; }
        public int TotalPackages { get; set; }
        public int PackageNumber { get; set; }
        public string FileId { get; set; }
    }
}
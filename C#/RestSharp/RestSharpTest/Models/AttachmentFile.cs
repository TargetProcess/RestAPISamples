using System.IO;

namespace RestSharpTest.Models
{
    public class AttachmentFile
    {
        public string FileName { get; set; }
        public MemoryStream Content { get; set; }
        public string ContentType { get; set; }
    }
}

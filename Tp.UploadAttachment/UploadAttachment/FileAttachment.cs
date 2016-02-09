using System.IO;

namespace UploadAttachment
{
    public class FileAttachment
    {
        public FileAttachment()
        {
            OriginalName = string.Empty;
            Description = string.Empty;
        }

        public string OriginalName { get; set; }
        public string Description { get; set; }
        public Stream FileStream { get; set; }
    }
}

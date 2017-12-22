using System.IO;

namespace RestSharpTest.Models
{
    public class AttachmentFile
    {
        public string FileName { get; set; }
        public MemoryStream Content { get; set; }
        public string ContentType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object[])")]
        public override string ToString() =>
            $"{nameof(FileName)}: {FileName}, {nameof(ContentType)}: {ContentType}";
    }
}

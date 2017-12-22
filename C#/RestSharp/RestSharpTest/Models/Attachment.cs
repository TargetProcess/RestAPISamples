using System;

namespace RestSharpTest.Models
{
    public class Attachment : ResourceBase
    {
        public Uri Uri { get; set; }
        public Uri ThumbnailUri { get; set; }
        public Request General { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object[])")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object,System.Object)")]
        public override string ToString() =>
            $"{base.ToString()}, {nameof(Uri)} : {Uri}, " +
            $"{nameof(ThumbnailUri)}: {ThumbnailUri}, {nameof(General)}: {{{General}}}";
    }
}

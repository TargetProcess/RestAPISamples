using System;

namespace RestSharpTest.Models
{
    public class Attachment
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public Uri Uri { get; set; }
        public Uri ThumbnailUri { get; set; }
        public Request General { get; set; }
    }
}

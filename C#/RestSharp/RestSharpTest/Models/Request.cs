namespace RestSharpTest.Models
{
    public class Request : ResourceBase
    {
        public string Description { get; set; }
        public Project Project { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object[])")]
        public override string ToString() =>
            $"{base.ToString()}, {nameof(Description)}: {Description}, {nameof(Project)}: {{{Project}}}";
    }
}

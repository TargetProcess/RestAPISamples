namespace RestSharpTest.Models
{
    public class ResourceBase
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object[])")]
        public override string ToString() => $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}";
    }
}

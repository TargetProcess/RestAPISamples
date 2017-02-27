namespace RestSharpTest.Models
{
    public class Request
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Project Project { get; set; }
    }
}

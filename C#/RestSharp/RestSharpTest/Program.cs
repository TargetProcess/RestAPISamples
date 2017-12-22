using RestSharp;
using RestSharp.Authenticators;
using RestSharpTest.Models;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace RestSharpTest
{
    class Program
    {
        // Requires C# version 7.1 or later.
        static async Task Main()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

            var client = new RestClient("https://md5.tpondemand.com/")
            {
                // See https://dev.targetprocess.com/docs/authentication for details.
                Authenticator = new HttpBasicAuthenticator("admin", "admin")
            };
            var createRequestResponse = await CreateRequest(client);

            Console.WriteLine($"Request creation status code: {createRequestResponse.StatusCode}");
            Console.WriteLine($"Request creation content: {createRequestResponse.Content}");

            var createdRequest = createRequestResponse.Data;
            Console.WriteLine($"Request creation data: {createdRequest}");

            var file = new AttachmentFile
            {
                FileName = "landscape.jpg",
                ContentType = "image/jpg",
                Content = new MemoryStream(File.ReadAllBytes(@"c:\landscape.jpg"))
            };

            var uploadAttachmentResponse = await UploadAttachment(client, file, createdRequest.Id ??
                throw new Exception("Reponse for Request creation has no Id field."));

            Console.WriteLine($"Attachment upload status code: {uploadAttachmentResponse.StatusCode}");
            Console.WriteLine($"Attachment upload content: {uploadAttachmentResponse.Content}");

            var uploadedAttachment = createRequestResponse.Data;
            Console.WriteLine($"Attachment upload data: {uploadedAttachment}");
        }

        private static Task<IRestResponse<Request>> CreateRequest(IRestClient client)
        {
            // See https://md5.tpondemand.com/api/v1/Index/meta for details.
            var restRequest = new RestRequest("/api/v1/requests", Method.POST);
            restRequest.AddHeader("Content-Type", "application/json; charset=utf-8");

            var request = new Request
            {
                Name = "Test Request created from API/v1 call",
                Description = "Test Request Description",
                Project = new Project {Id = 1584}
            };

            restRequest.AddBody(request);
            return client.ExecutePostTaskAsync<Request>(restRequest);
        }

        private static Task<IRestResponse<Attachment>> UploadAttachment(IRestClient client, AttachmentFile file, int id)
        {
            var restRequest = new RestRequest("UploadFile.ashx", Method.POST);
            restRequest.AddHeader("Content-Type", "multipart/form-data");
            restRequest.AddFile("attachment", file.Content.ToArray(), file.FileName, file.ContentType);
            restRequest.AddParameter("generalId", id);

            return client.ExecutePostTaskAsync<Attachment>(restRequest);
        }
    }
}

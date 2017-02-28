using RestSharp;
using RestSharp.Authenticators;
using RestSharpTest.Models;
using System;
using System.IO;
using System.Net;

namespace RestSharpTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient("https://julia.tpondemand.com/");
            client.Authenticator = new HttpBasicAuthenticator("admin", "admin");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

            var request = CreateRequest(client);
            var file = new AttachmentFile()
            {
                FileName = "landscape.jpg",
                ContentType = "image/jpg",
                Content = new MemoryStream(File.ReadAllBytes(@"c:\landscape.jpg"))
            };
            UploadAttachment(client, file, request.Id.Value);
        }

        private static Request CreateRequest(RestClient client)
        {
            var restRequest = new RestRequest("/api/v1/requests", Method.POST);
            restRequest.AddHeader("Content-Type", "application/json; charset=utf-8");

            var request = new Request();
            request.Name = "test";
            request.Description = "test description";
            request.Project = new Project { Id = 1524 };

            restRequest.AddBody(request);
            var response = client.Execute<Request>(restRequest);
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(response.Content);

            return response.Data;
        }

        private static Attachment UploadAttachment(RestClient client, AttachmentFile file, int id)
        {
            var restRequest = new RestRequest("UploadFile.ashx", Method.POST);
            restRequest.AddHeader("Content-Type", "multipart/form-data");
            restRequest.AddFile("attachment", file.Content.ToArray(), file.FileName, file.ContentType);
            restRequest.AddParameter("generalId", id);

            var response = client.Execute<Attachment>(restRequest);
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(response.Content);

            return response.Data;
        }
    }
}

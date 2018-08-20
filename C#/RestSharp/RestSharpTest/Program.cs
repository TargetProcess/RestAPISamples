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
        static void Main()
        {
            var client = new RestClient("https://restapi.tpondemand.com/");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

            //client.Authenticator = new HttpBasicAuthenticator("John", "123"); //basic authentication
            //var token = "?token=Njo4OTIyQzkzN0M5NEY3NzNENDIyNTM2RDU3MTMwMTMwOA=="; //token
            var token = "?access_token=NjowTGZHR1AwWGV6VkJ3VVFvcGU5RHF0R09MNkJ2Y1RnckNQVnRrQjYxYUdRPQ=="; //access_token
            //more about authentication at https://dev.targetprocess.com/docs/authentication

            var request = CreateRequest(client, token);
            var file = new AttachmentFile()
            {
                FileName = "download.png",
                ContentType = "image/png",
                Content = new MemoryStream(File.ReadAllBytes(@"c:\Work\download.png"))
            };

            UploadAttachment(client, token, file, request.Id.Value);
        }

        private static Request CreateRequest(RestClient client, string token)
        {
            var restRequest = new RestRequest("/api/v1/requests" + token, Method.POST);
            restRequest.AddHeader("Content-Type", "application/json; charset=utf-8");
            var request = new Request();
            request.Name = "New request";
            request.Description = "test description";
            request.Project = new Project { Id = 13 };
            restRequest.AddBody(request);
            var response = client.Execute<Request>(restRequest);
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(response.Content);
            return response.Data;
        }

        private static Attachment UploadAttachment(RestClient client, string token, AttachmentFile file, int id)
        {
            var restRequest = new RestRequest("UploadFile.ashx" + token, Method.POST);
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

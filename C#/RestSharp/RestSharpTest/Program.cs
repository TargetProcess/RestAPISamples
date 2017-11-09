using RestSharp;
using RestSharp.Authenticators;
using RestSharpTest.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RestSharpTest
{
    class Program
    {
        // ** Change connection strings accordingly
        const string ChromeExe = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
        private const string DomainUrl = "http://local.tsarevich.tp.com/";

        static void Main()
        {
            var client = new RestClient(DomainUrl)
            {
                Authenticator = new HttpBasicAuthenticator("admin", "admin")
            };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

            var tasks = new List<Task<IRestResponse<Request>>>(34);
            tasks.AddRange(Enumerable.Range(1, 48).Select(no => CreateRequest(client, no)));

            var wasCreated = Task.WaitAll(tasks.Select(t => (Task)t).ToArray(), new TimeSpan(0, 2, 0));

            Console.WriteLine(wasCreated);
        }

        private static Task<IRestResponse<Request>> CreateRequest(RestClient client, int no)
        {
            var restRequest = new RestRequest("/api/v1/requests", Method.POST);
            restRequest.AddHeader("Content-Type", "application/json; charset=utf-8");
            restRequest.AddQueryParameter("access_token",
                "MTpKVFlEZmVjdW9KUWhOOWxGOFdjR0ZlUmYwWldYRUlJa0lZL0JXZE5HV1g0PQ==");

            var request = new Request
            {
                Name = $"test_{no}",
                Description = "test description",
                Project = new Project {Id = 1087}
            };

            restRequest.AddBody(request);
            return client.ExecuteTaskAsync<Request>(restRequest)
                .ContinueWith(createRequest =>
                {
                    //if (createRequest.Result.StatusCode == HttpStatusCode.Created)
                    //{
                    //    Process.Start(ChromeExe,
                    //        $"{DomainUrl}entity/{createRequest.Result.Data.Id}");
                    //}

                    return createRequest.Result;
                });
        }
    }
}

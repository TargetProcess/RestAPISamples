using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Serialization;

namespace TpRestExample
{
	static class Program
	{
		private const string PathToTp = "http://localhost/TargetProcess/";

		static void Main()
		{
			ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

			using (var client = new WebClient
			{
				Credentials = new NetworkCredential("admin", "admin"), // Use Basic Authentication
				//Proxy = new WebProxy("fiddler-proxy",8888)
			})
			{
				var userStoryCollection = DownloadUserStories(client);

				foreach (var story in userStoryCollection.UserStories)
				{
					Console.WriteLine("{0} {1} {2}", story.Id, story.Name, story.Project);
				}

				var firstStory = userStoryCollection.UserStories.First();
				firstStory.Name = "New name";

				var result = UpdateUserStory(firstStory, client);
				Console.WriteLine(result);
			}
		}

		private static string UpdateUserStory(UserStory firstStory, WebClient client)
		{
			var ns = new XmlSerializerNamespaces();
			ns.Add("", "");
			var storySerializer = new XmlSerializer(typeof (UserStory));
			var output = new StringWriter();

			storySerializer.Serialize(output, firstStory, ns);
			var serialized = output.ToString();

			var result = client.UploadString(PathToTp + string.Format("api/v1/UserStories/{0}?include=[Id,Name,Project[Id,Name]]", firstStory.Id),
			                                 "POST", serialized);
			return result;
		}

		private static UserStoryCollection DownloadUserStories(WebClient client)
		{
			string xml = client.DownloadString(PathToTp + "api/v1/UserStories?include=[Id,Name,Project[Id,Name]]&where=Project.Id eq 2");

			var serializer = new XmlSerializer(typeof (UserStoryCollection));
			var userStoryCollection = (UserStoryCollection) serializer.Deserialize(new StringReader(xml));
			return userStoryCollection;
		}
	}
}

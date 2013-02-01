using System.Xml.Serialization;

namespace TpRestExample
{
	[XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "UserStories")]
	public class UserStoryCollection
	{
		public UserStoryCollection()
		{
			UserStories = new UserStory[0];
		}

		[XmlElement("UserStory")]
		public UserStory[] UserStories { get; set; }

		[XmlAttribute]
		public string Next { get; set; }
	}

	[XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "UserStory")]
	public class UserStory
	{
		[XmlAttribute]
		public int Id { get; set; }

		[XmlAttribute]
		public string Name { get; set; }

		public Project Project { get; set; }
	}

	[XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "Project")]
	public class Project
	{
		[XmlAttribute]
		public int Id { get; set; }

		[XmlAttribute]
		public string Name { get; set; }

		public override string ToString()
		{
			return string.Format("Id: {0}, Name: {1}", Id, Name);
		}
	}


}
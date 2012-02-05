using Divan;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using ObjectModel;

namespace CouchDBTest
{
	[TestFixture]
	public class SerializationTests
	{
		public SerializationTests ()
		{
		}
		
		[Test]
		public void SerializeAndDeserialize_CheckIfSerializationWorks_Works()
		{
			ItemList list = new ItemList("at0001");
			list.Items.Add(new DvText("at0002", new CString("#Test")));
			list.Items.Add(new DvCount("at0003", new CCount(25)));

			string json = CouchDocument.WriteJson(list);

			JObject obj = (JObject)JsonConvert.DeserializeObject(json); 
			
			ItemList deserializedList = new ItemList();
			deserializedList.ReadJson(obj);

			Assert.AreEqual(list.Items[0].TypeName, deserializedList.Items[0].TypeName);
			Assert.AreEqual(list.Items[1].TypeName, deserializedList.Items[1].TypeName);
			Assert.AreEqual(((DvText)list.Items[0]).Value.Value, ((DvText)deserializedList.Items[0]).Value.Value);
			Assert.AreEqual(((DvCount)list.Items[1]).Value.Magnitude, ((DvCount)deserializedList.Items[1]).Value.Magnitude);
		}
	}
}


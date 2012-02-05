using Divan;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ObjectModel
{
	public abstract class OMBase: CouchDocument
	{
		private string _archetypeNodeID = null;
		
		public OMBase()
		{
			Id = "";
			Rev = "";
		}
		
		public OMBase (string archetypeNodeID)
			:this()
		{
			ArchetypeNodeID = archetypeNodeID;
		}
		
		public string ArchetypeNodeID
		{
			get { return _archetypeNodeID; }
			set { _archetypeNodeID = value; }
		}
		
		public abstract string TypeName
		{
			get;
		}
		
		public override void WriteJson(JsonWriter writer)
		{
			base.WriteJson(writer);
			
			writer.WritePropertyName("type_name");
			writer.WriteValue(TypeName);
			writer.WritePropertyName("archetype_node_id");
			writer.WriteValue(ArchetypeNodeID);
			
			OnWriteJson(writer);
		}
		
		public override void ReadJson(JObject obj)
		{
			base.ReadJson(obj);
			
			//TypeName = obj["type_name"].Value<string>();
			ArchetypeNodeID = obj["archetype_node_id"].Value<string>();
			
			OnReadJson(obj);
		}
		
		public abstract void OnWriteJson(JsonWriter writer);
		public abstract void OnReadJson(JObject obj);
	}
}


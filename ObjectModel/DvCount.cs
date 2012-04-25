using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ObjectModel
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public class DvCount: OMBase
	{
		private CCount _value = null;
		
		public DvCount ()
			:base()
		{
		}
		
		public DvCount(string archetypeNodeID)
			:base(archetypeNodeID)
		{
			
		}
		
		public DvCount(string archetypeNodeID, CCount value)
			:base(archetypeNodeID)
		{
			Value = value;
		}
		
		public override string TypeName 
		{
			get { return "DV_COUNT"; }
		}
		
		[JsonProperty("value")]
		public CCount Value 
		{
			get { return _value; }
			set { _value = value; }
		}
		
		public override void OnWriteJson (JsonWriter writer)
		{
			writer.WritePropertyName("value");

			writer.WriteStartObject();

			if (Value != null)
				Value.WriteJson(writer);

			writer.WriteEndObject();
		}
		
		public override void OnReadJson (JObject obj)
		{
			Value = new CCount();

			Value.ReadJson((JObject)obj["value"]);
		}
	}
}


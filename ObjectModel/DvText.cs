using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ObjectModel
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public class DvText: OMBase
	{
		private CString _value = null;
		
		public DvText ()
			:base()
		{
		}
		
		public DvText(string archetypeNodeID) 
			: base(archetypeNodeID)
		{
			
		}
		
		public DvText(string archetypeNodeID, CString value)
			: base (archetypeNodeID)
		{
			Value = value;
		}

		public override string TypeName 
		{
			get { return "DV_TEXT";	}
		}
		
		[JsonProperty("value")]
		public CString Value
		{
			get { return _value; }
			set { _value = value; }
		}
		
		public override void OnWriteJson (JsonWriter writer)
		{
			writer.WritePropertyName("value");

			writer.WriteStartObject();

			if(Value != null)
				Value.WriteJson(writer);

			writer.WriteEndObject();
		}
		
		public override void OnReadJson (JObject obj)
		{
			Value = new CString();

			Value.ReadJson((JObject)obj["value"]);
		}

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    if (_value != null)
                    {
                        _value.Dispose();
                    }
                }

                base.Dispose(disposing);
            }
        }
	}
}


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ObjectModel
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public class CString : CPrimitive
	{
		private string _value = null;
		
		public CString ()
		{
		}
		
		public CString(string value)
		{
			Value = value;
		}
		
		[JsonProperty("value")]
		public string Value
		{
			get { return _value; }
			set { _value = value; }
		}
		
		public override void WriteJson(JsonWriter writer)
		{
			writer.WritePropertyName("value");
			writer.WriteValue(Value);
		}
		
		public override void ReadJson(JObject obj)
		{
			Value = obj["value"].Value<string>();
		}

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    _value = null;
                }

                base.Dispose(disposing);
            }
        }
	}
}


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ObjectModel
{
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
	}
}


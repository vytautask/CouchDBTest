using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ObjectModel
{
	public class CCount: CPrimitive
	{
		private int _magnitude = 0;
		
		public CCount ()
		{
		}
		
		public CCount(int magnitude)
		{
			Magnitude = magnitude;
		}
		
		public int Magnitude
		{
			get { return _magnitude; }
			set { _magnitude = value; }
		}
		
		public override void WriteJson(JsonWriter writer)
		{
			writer.WritePropertyName("magnitude");
			writer.WriteValue(Magnitude);
		}
		
		public override void ReadJson(JObject obj)
		{
			Magnitude = obj["magnitude"].Value<int>();
		}
	}
}


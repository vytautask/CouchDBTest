using System;
using Divan;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ObjectModel
{
	public abstract class CPrimitive: ICanJson
	{
		public CPrimitive ()
		{
		}		
		
		public abstract void WriteJson(JsonWriter writer);
		public abstract void ReadJson(JObject obj);
	}
}


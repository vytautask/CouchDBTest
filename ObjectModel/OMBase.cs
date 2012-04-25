using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using LoveSeat;
using System.Runtime.Serialization;

namespace ObjectModel
{
	public abstract class OMBase : Document, IDisposable //CouchDocument, 
	{
		private string _archetypeNodeID = null;
        private bool _isDisposed = false;

		public OMBase()
		{
		}
		
		public OMBase (string archetypeNodeID)
			:this()
		{
			ArchetypeNodeID = archetypeNodeID;
		}

		[JsonProperty("archetype_node_id")]
		public string ArchetypeNodeID
		{
			get { return _archetypeNodeID; }
			set { _archetypeNodeID = value; }
		}

		[JsonProperty("type_name")]
		public abstract string TypeName
		{
			get;
		}

        public bool IsDisposed
        {
            get { return _isDisposed; }
            private set { _isDisposed = value; }
        }
        
		public void WriteJson(JsonWriter writer)
		{
			if (!string.IsNullOrEmpty(Id))
			{
				writer.WritePropertyName("_id");
				writer.WriteValue(Id);
			}

			if (!string.IsNullOrEmpty(Rev))
			{
				writer.WritePropertyName("_rev");
				writer.WriteValue(Rev);
			}
			
			writer.WritePropertyName("type_name");
			writer.WriteValue(TypeName);
			writer.WritePropertyName("archetype_node_id");
			writer.WriteValue(ArchetypeNodeID);
			
			OnWriteJson(writer);
		}
		
		public void ReadJson(JObject obj)
		{
			if (obj["_id"] != null)
			{
				Id = obj["_id"].Value<string>();
			}

			if (obj["_rev"] != null)
			{
				Rev = obj["_rev"].Value<string>();
			}

			ArchetypeNodeID = obj["archetype_node_id"].Value<string>();
			
			OnReadJson(obj);
		}
		
		public abstract void OnWriteJson(JsonWriter writer);
		public abstract void OnReadJson(JObject obj);


        protected virtual void Dispose(bool disposing)
		{
			if (!IsDisposed)
			{
				if (disposing)
				{
                    _archetypeNodeID = null;
				}

				IsDisposed = false;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~OMBase()
		{
			Dispose(false);
		}
	}
}


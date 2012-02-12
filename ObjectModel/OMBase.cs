using Divan;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace ObjectModel
{
	public abstract class OMBase: CouchDocument, IDisposable
	{
		private string _archetypeNodeID = null;
        private bool _isDisposed = false;
		private bool _useIDAndRev = true;
		
		public OMBase()
		{
			//Id = "";
			//Rev = "";
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
		
		public bool UseIDAndRev
		{
			get { return _useIDAndRev; }
			set { _useIDAndRev = value; }
		}

		public abstract string TypeName
		{
			get;
		}

        public bool IsDisposed
        {
            get { return _isDisposed; }
            private set { _isDisposed = value; }
        }
        
		public override void WriteJson(JsonWriter writer)
		{
			if(UseIDAndRev)
				base.WriteJson(writer);
			
			writer.WritePropertyName("type_name");
			writer.WriteValue(TypeName);
			writer.WritePropertyName("archetype_node_id");
			writer.WriteValue(ArchetypeNodeID);
			
			OnWriteJson(writer);
		}
		
		public override void ReadJson(JObject obj)
		{
			if(UseIDAndRev)
				base.ReadJson(obj);
			
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


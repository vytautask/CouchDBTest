using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ObjectModel
{
	public class ItemList: OMBase
	{
		private IList<OMBase> _items = null;
		
		public ItemList ()
		{
		}
		
		public ItemList(string archetypeNodeID)
			:base(archetypeNodeID)
		{
			
		}
		
		public IList<OMBase> Items
		{
			get 
			{
				if(_items == null)
					_items = new List<OMBase>();
				
				return _items;
			}
			set { _items = value; }
		}

		public override string TypeName 
		{
			get { return "ITEM_LIST"; }
		}
		
		public override void OnWriteJson (JsonWriter writer)
		{
			writer.WritePropertyName("items");

			writer.WriteStartArray();

			int count = Items.Count;
			StringBuilder serialized = new StringBuilder();
			for(int i = 0; i < count; i++)
			{
				writer.WriteStartObject();
				Items[i].WriteJson(writer);
				writer.WriteEndObject();
			}

			writer.WriteEndArray();
		}
		
		public override void OnReadJson (JObject obj)
		{
			Items.Clear();

			IEnumerator<JToken> enumerator = obj["items"].Children().GetEnumerator();

			while (enumerator.MoveNext())
			{
				OMBase item = TypeResolver.ResolveType(enumerator.Current["type_name"].Value<string>());
				item.UseIDAndRev = UseIDAndRev;

				item.ReadJson((JObject)enumerator.Current);

				Items.Add(item);
			}
		}

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    for (int i = 0; i < Items.Count; i++)
                    {
                        Items[i].Dispose();
                    }

                    _items = null;
                }

                base.Dispose(disposing);
            }
        }
	}
}


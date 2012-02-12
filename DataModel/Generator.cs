using System;
using System.Collections.Generic;
using ObjectModel;
using Divan;
using System.IO;
using System.Text;

namespace DataModel
{
	public class Generator
	{
		public static void GenerateRandomObjects(int count, string file)
		{
			Random rnd = new Random();

			using (StreamWriter outfile = new StreamWriter(file, false, Encoding.UTF8))
			{
				for (int i = 0; i < count; i++)
				{
					ItemList list = new ItemList("someName " + rnd.Next().ToString());
					list.Rev = null;

					int itemCount = rnd.Next(100);

					for (int j = 0; j < itemCount; j++)
					{
						OMBase item = null;
						if (j % 2 == 0)
						{
							item = new DvText("textNode_id_" + rnd.Next().ToString(), new CString(rnd.Next().ToString() + " test string " + rnd.Next().ToString()));
						}
						else
						{
							item = new DvCount("countNode_id_" + rnd.Next().ToString(), new CCount(rnd.Next()));
						}

						item.Rev = null;

						list.Items.Add(item);
					}

					outfile.WriteLine(CouchDocument.WriteJson(list));
					list.Dispose();
					list = null;
				}
			}
		}
	}
}

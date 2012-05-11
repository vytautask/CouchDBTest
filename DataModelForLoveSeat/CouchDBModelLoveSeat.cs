using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Linq;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ObjectModel;
using LoveSeat;

namespace DataModel
{
	public class CouchDBModelLoveSeat
	{
		private CouchClient _client = null;
		private CouchDatabase _database = null;
		private static readonly ILog _log = LogManager.GetLogger(typeof(CouchDBModelLoveSeat));

		public CouchDBModelLoveSeat(string host, int port, string databaseName, bool renewDatabase)
		{
			_client = new CouchClient();

			if (renewDatabase && _client.HasDatabase(databaseName))
					_client.DeleteDatabase(databaseName);

			_client.CreateDatabase(databaseName);

			_database = _client.GetDatabase(databaseName);
		}

		public void CreateObject(OMBase obj)
		{
			_database.CreateDocument(obj);
		}

		public void SaveObject(OMBase obj)
		{
			_database.SaveDocument(obj);
		}

		public T GetObject<T>(string id)
			where T: Document, new()
		{
			return _database.GetDocument<T>(id);
		}

		public void SaveAllObjects(string file)
		{
			_log.Info("Starting to save objects to DB");

			List<long> times = new List<long>();

			Stopwatch watch = new Stopwatch();
			using (StreamReader sr = new StreamReader(file, Encoding.UTF8))
			{
				watch.Start();
				while (!sr.EndOfStream)
				{
					JObject obj = (JObject)JsonConvert.DeserializeObject(sr.ReadLine());

					ItemList deserializedList = new ItemList();
					deserializedList.ReadJson(obj);

					SaveObject(deserializedList);
					times.Add(watch.ElapsedMilliseconds);

					deserializedList.Dispose();
					deserializedList = null;
					obj = null;
				}
				watch.Stop();
				watch = null;
			}

			_log.Info("Calculating average time...");

			double average = 0;
			for (int i = 0; i < times.Count; i++)
			{
				average += times[i];
			}

			average = average / times.Count;

			_log.InfoFormat("Average insertion time: {0} milliseconds", average);
		}

		public ItemList GetRandomItemListFromDataFile(string dataFile)
		{
			string[] lines = File.ReadAllLines(dataFile);

			JObject obj = (JObject)JsonConvert.DeserializeObject(lines[lines.Length / 2]);

			ItemList deserializedList = new ItemList();
			deserializedList.ReadJson(obj);

			return deserializedList;
		}

		public void PerformSelect(string dataFile)
		{
			//ICouchViewDefinition tempView = _database.NewTempView("test", "test", "if (doc.docType && doc.docType == 'ITEM_LIST') emit(doc._id, doc);");
			//ICouchViewDefinition tempView = _database.NewTempView("test", "test", "emit(doc.archetype_node_id, doc);");

			ItemList randomItem = GetRandomItemListFromDataFile(dataFile);

			ViewOptions options = new ViewOptions();
			options.Key.Add(randomItem.ArchetypeNodeID);
			options.IncludeDocs = true;
			Stopwatch watch = new Stopwatch();
			watch.Start();

			_log.Info("Starting select 1");

			ViewResult result = _database.View("test_archetype_node_id", options, "test");
			
			watch.Stop();

			int count = result.Rows.Count<JToken>(); ;

			_log.InfoFormat("Select done in {0}ms and found {1} documents", watch.Elapsed.TotalMilliseconds, count);
		}

		public void PerformSelect2(string dataFile)
		{
			ItemList randomItem = GetRandomItemListFromDataFile(dataFile);

			ViewOptions options = new ViewOptions();
			options.Key.Add(new JRaw("[ \"" + randomItem.Items[3].ArchetypeNodeID + "\", null, \"" + randomItem.Items[3].TypeName + "\"]"));
			options.IncludeDocs = true;
			Stopwatch watch = new Stopwatch();
			watch.Start();

			_log.Info("Starting select 2");

			ViewResult result = _database.View("archetype_node_id_value", options, "test");

			watch.Stop();

			int count = result.Rows.Count<JToken>(); ;

			_log.InfoFormat("Select2 done in {0}ms and found {1} documents", watch.Elapsed.TotalMilliseconds, count);
		}
	}
}


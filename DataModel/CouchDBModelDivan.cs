using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Linq;
using Divan;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ObjectModel;

namespace DataModel
{
	public class CouchDBModelDivan
	{
		private CouchServer _server = null;
		private ICouchDatabase _database = null;
		private static readonly ILog _log = LogManager.GetLogger(typeof(CouchDBModelDivan));

		public CouchDBModelDivan (string host, int port, string databaseName, bool renewDatabase)
		{
			_server = new CouchServer(host, port);

			if (renewDatabase && _server.HasDatabase(databaseName))
					_server.DeleteDatabase(databaseName);

			_database = _server.GetDatabase(databaseName);
		}

		public void SaveObject(ICouchDocument obj)
		{
			_database.SaveDocument(obj);
			//_database.CreateDocument(obj);
		}

		public T GetObject<T>(string id)
			where T: ICouchDocument, new()
		{
			return _database.GetDocument<T>(id);
		}

		public IEnumerable<T> GetAllObjects<T>()
			where T: ICouchDocument, new()
		{
			return _database.GetAllDocuments<T>();
		}

		public IEnumerable GetByArchetypeNodeIDAndValue(string nodeID, string value)
		{
			throw new NotImplementedException();
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
					//_database.SaveDocument(deserializedList);
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
			ICouchViewDefinition tempView = _database.NewTempView("test", "test1", "if (doc.archetype_node_id) { emit(doc.archetype_node_id, doc); }");
			//ICouchViewDefinition tempView = _database.NewTempView("test", "test", "emit(doc.ArchetypeNodeID, { ArchetypeNodeID: doc.ArchetypeNodeID });");

			ItemList randomItem = GetRandomItemListFromDataFile(dataFile);

			var linDocuments = tempView.LinqQuery<ItemList>();
			_log.Info("Starting select 1");

			Stopwatch watch = new Stopwatch();
			watch.Start();

			//IEnumerable<ItemList> results = tempView.Query().Key(deserializedList.ArchetypeNodeID).GetResult().Documents<ItemList>();
			//IList<ItemList> results = new List<ItemList>(from c in linDocuments where c.Items[11].ArchetypeNodeID == "countNode_id_975622625" select c);
			//IList<ItemList> results = new List<ItemList>(from c in linDocuments where c.ArchetypeNodeID == deserializedList.ArchetypeNodeID select c);

			IList<ItemList> results = new List<ItemList>(from c in linDocuments where c.ArchetypeNodeID == randomItem.ArchetypeNodeID select c);

			watch.Stop();

			_log.InfoFormat("Select done in {0}s and found {1} documents", watch.Elapsed.TotalSeconds, results.Count<ItemList>());
		}

		public void PerformSelect2(string dataFile)
		{
			ICouchViewDefinition tempView = _database.NewTempView("test", "test2", "if (doc.items) { emit(doc.archetype_node_id, doc); }");

			ItemList randomItem = GetRandomItemListFromDataFile(dataFile);

			var linDocuments = tempView.LinqQuery<ItemList>();
			_log.Info("Starting select 2");

			//Stopwatch watch = new Stopwatch();
			//watch.Start();

			//IList<ItemList> results = new List<ItemList>(from c in linDocuments where  select c);

			//watch.Stop();

			//_log.InfoFormat("Select 2 done in {0}s and found {1} documents", watch.Elapsed.TotalSeconds, results.Count<ItemList>());
		}
	}
}


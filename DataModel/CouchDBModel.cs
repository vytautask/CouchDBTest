using System;
using Divan;
using ObjectModel;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using log4net;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

namespace DataModel
{
	public class CouchDBModel
	{
		private CouchServer _server = null;
		private ICouchDatabase _database = null;
		private static readonly ILog _log = LogManager.GetLogger(typeof(CouchDBModel));

		public CouchDBModel (string host, int port, string databaseName)
		{
			_server = new CouchServer(host, port);
			_database = _server.GetDatabase(databaseName);
		}

		public void SaveObject(ICouchDocument obj)
		{
			//_database.SaveDocument(obj);
			_database.CreateDocument(obj);
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
			_log.Info("starting to save objects to DB");

			List<long> times = new List<long>();

			Stopwatch watch = new Stopwatch();
			using (StreamReader sr = new StreamReader(file, Encoding.UTF8))
			{
				watch.Start();
				while (!sr.EndOfStream)
				{
					JObject obj = (JObject)JsonConvert.DeserializeObject(sr.ReadLine());

					ItemList deserializedList = new ItemList();
					deserializedList.UseIDAndRev = false;
					deserializedList.ReadJson(obj);

					watch.Reset();
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

			_log.InfoFormat("Average insertion time: {0} milliseconds", average);
		}
	}
}


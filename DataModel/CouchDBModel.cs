using System;
using Divan;
using ObjectModel;
using System.Collections.Generic;
using System.Collections;

namespace DataModel
{
	public class CouchDBModel
	{
		private CouchServer _server = null;
		private ICouchDatabase _database = null;

		public CouchDBModel (string host, int port, string databaseName)
		{
			_server = new CouchServer(host, port);
			_database = _server.GetDatabase(databaseName);

			Console.WriteLine("Saved 10 Cars with 170-180 hps each.");

		}

		public void SaveObject(ICouchDocument obj)
		{
			_database.SaveDocument(obj);
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

		public void GenerateRandomRows(int count)
		{
			throw new NotImplementedException();
		}
	}
}


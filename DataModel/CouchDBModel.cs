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

			
			// Load all Cars. If we dwelve into the GetAllDocuments() method we will see that
			// QueryAllDocuments() gives us a CouchQuery which we can configure. We tell it to IncludeDocuments()
			// which means that we will get back not only ids but the actual documents too. GetResult() will perform the
			// HTTP request to CouchDB and return a CouchGenericViewResult which we in turn can ask to produce objects from JSON,
			// in this case we pick out the actual documents and instantiate them as instances of the class Car.
			var cars = db.GetAllDocuments<Car>();
			Console.WriteLine("Loaded all Cars: " + cars.Count());

			// Now try some linq
			var tempView = db.NewTempView("test", "test", "if (doc.docType && doc.docType == 'car') emit(doc.Hps, doc);");
			var linqCars = tempView.LinqQuery<Car>();

			var fastCars = from c in linqCars where c.HorsePowers >= 175 select c;//.Make + " " + c.Model;
			foreach (var fastCar in fastCars)
				Console.WriteLine(fastCar);

			var twoCars = from c in linqCars where c.HorsePowers == 175 || c.HorsePowers == 176 select c;//.Make + " " + c.Model;
			foreach (var twoCar in twoCars)
				Console.WriteLine(twoCar);


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


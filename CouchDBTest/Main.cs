using System;
using ObjectModel;
using Divan;
using Newtonsoft.Json;
using log4net.Config;
using DataModel;
using log4net;

namespace CouchDBTest
{
	class MainClass
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(MainClass));

		public static void Main (string[] args)
		{
			Console.WriteLine ("Starting Tests...");

			Configure();

			TestInsertion();
		}

		private static void TestInsertion()
		{
			CouchDBModel model = new CouchDBModel("127.0.0.1", 5984, "ehr_test_db");

			_log.Info("starting to generate random objects");
			Generator.GenerateRandomObjects(10000000, "data.json");
			//Generator.GenerateRandomObjects(5, "data.json");
			_log.Info("objects generated");

			model.SaveAllObjects("data.json");
		}

		private static void Configure()
		{
			XmlConfigurator.Configure();
		}
	}
}

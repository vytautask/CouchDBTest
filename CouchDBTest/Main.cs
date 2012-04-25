using System;
using ObjectModel;
using Newtonsoft.Json;
using log4net.Config;
using log4net;
using DataModel;

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
			CouchDBModelLoveSeat model = new CouchDBModelLoveSeat("127.0.0.1", 5984, "ehr_test_db", false);

			string tempDataFile = "data.json";

			_log.Info("starting to generate random objects");
			//Generator.GenerateRandomObjects(10000000, tempDataFile);
			//Generator.GenerateRandomObjects(10000, tempDataFile);
			//Generator.GenerateRandomObjects(5, tempDataFile);
			_log.Info("objects generated");

			//model.SaveAllObjects(tempDataFile);

			model.PerformSelect(tempDataFile);
		}

		private static void Configure()
		{
			XmlConfigurator.Configure();
		}
	}
}

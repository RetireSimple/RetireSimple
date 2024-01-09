using MongoDB.Bson;
using MongoDB.Driver;

namespace RetireSimple.NewTests {
	[TestClass]
	public class TestDBConnection {
		[TestMethod]
		public void TestDBConnection1() {
			var connectionString = "mongodb://localhost:27017";

			//take database name from connection string
			var _databaseName = MongoUrl.Create(connectionString).DatabaseName;
			var _client = new MongoClient(connectionString);
			var _collection = _client.GetDatabase("local").GetCollection<BsonDocument>("RetireSimple");
			var _filter = Builders<BsonDocument>.Filter.Eq("cash", 27004);
			var _document = _collection.Find(_filter).First();
			Console.WriteLine(_document);
		}
	}
}
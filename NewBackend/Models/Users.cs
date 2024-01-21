using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NewBackend.Models {

	public class Users {

		// age, retirementAge, retirementGoal, filingStatus
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string? Id { get; set; }

		public int Age { get; set; }
		public int RetirementAge { get; set; }
		public double RetirementGoal { get; set; }

		public string FilingStatus { get; set; }





	}
}

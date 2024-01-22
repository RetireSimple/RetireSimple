using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NewBackend.Models;

using System.Runtime.CompilerServices;

namespace NewBackend.Services {
	public class UsersService {
		private readonly IMongoCollection<Users> _usersCollection;
	

		public UsersService(
			IOptions<RetireSimpleDatabaseSettings> RetireSimpleDatabaseSettings) {
			var mongoClient = new MongoClient(
				RetireSimpleDatabaseSettings.Value.ConnectionString);

			var mongoDatabase = mongoClient.GetDatabase(
				RetireSimpleDatabaseSettings.Value.DatabaseName);

			_usersCollection = mongoDatabase.GetCollection<Users>(
				RetireSimpleDatabaseSettings.Value.UsersCollectionName);
		}

		public async Task<List<Users>> GetAsync() =>
			await _usersCollection.Find(_ => true).ToListAsync();

		public async Task<Users?> GetAsync(string id) =>
			await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

		public async Task CreateAsync(Users newUsers) =>
			await _usersCollection.InsertOneAsync(newUsers);

		public async Task UpdateAsync(string id, Users updatedUsers) =>
			await _usersCollection.ReplaceOneAsync(x => x.Id == id, updatedUsers);

		public async Task RemoveAsync(string id) =>
			await _usersCollection.DeleteOneAsync(x => x.Id == id);
	}
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RetireSimple.Engine.Data.User {
	public class Profile {

		public int ProfileId { get; set; }
		public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
		public string Name { get; set; }
		public int Age { get; set; }
		public bool Status { get; set; }

	}

	public class ProfileConfiguration : IEntityTypeConfiguration<Profile> {
		public void Configure(EntityTypeBuilder<Profile> builder) {
			builder.HasKey(p => p.ProfileId);
			builder.HasMany(p => p.Portfolios)
				.WithOne(p => p.Profile)
				.OnDelete(DeleteBehavior.Cascade);

			//NOTE this is a placeholder to guarantee an existing Profile/Portfolio until that feature reaches implementation
			builder.HasData(new { ProfileId = 1, Name = "Default", Age = 65, Status = true });
		}
	}

}

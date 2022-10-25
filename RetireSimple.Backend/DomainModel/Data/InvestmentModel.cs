using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RetireSimple.Backend.DomainModel.Data;

namespace RetireSimple.Backend.DomainModel.Data {
    public class InvestmentModel {
		public int InvestmentModelId { get; set; }
		//TODO add more statistics fields when we get there

		//TODO ensure relationship to InvestmentBase
		public int InvestmentId { get => InvestmentId; set => InvestmentId = value; }
		

		
        //TODO change to Math.NET/other types if needed
        List<(double, double)> MaxModelData { get => MaxModelData; set => MaxModelData = value; }
        List<(double, double)> MinModelData { get => MinModelData; set => MinModelData = value; }
    }

	public class InvestmentModelConfiguration : IEntityTypeConfiguration<InvestmentModel> {
		public void Configure(EntityTypeBuilder<InvestmentModel> builder) {
			builder.ToTable("InvestmentModel");
			builder.HasKey(i => i.InvestmentModelId);

			builder.HasAlternateKey(i => i.InvestmentId);
			
		}
	}
}

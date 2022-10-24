namespace RetireSimple.Backend.DomainModel.Data {
    public abstract class InvestmentModel {

        //TODO add more statistics fields when we get there


        //TODO ensure relationship to InvestmentBase
        int InvestmentId { get => InvestmentId; set => InvestmentId = value; }

        //TODO change to Math.NET/other types if needed
        List<(double, double)> MaxModelData { get => MaxModelData; set => MaxModelData = value; }
        List<(double, double)> MinModelData { get => MinModelData; set => MinModelData = value; }
    }
}

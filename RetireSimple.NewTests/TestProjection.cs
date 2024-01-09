using RetireSimple.Engine.New_Engine;

namespace RetireSimple.NewTests {
	[TestClass]
	public class TestProjection {
		[TestMethod]
		public void TestProjection1() {
			List<double> list1 = new List<double>();
			list1.Add(1.0);
			list1.Add(4.3);
			list1.Add(5.7);
			Projection projection1 = new Projection(list1, 0);

			list1.Add(7.0);
			Projection projection2 = new Projection(list1, 0);

			Projection projection3 = projection1.Add(projection2);


			List<double> result = new List<double>();

			result.Add(2.0);
			result.Add(8.6);
			result.Add(11.4);
			result.Add(7.0);

			projection3.yearly_projections.Equals(result);
		}

		[TestMethod]
		public void TestProjection2() {
			List<double> list1 = new List<double>();
			list1.Add(1.0);
			list1.Add(4.3);
			list1.Add(5.7);
			list1.Add(7.0);
			Projection projection1 = new Projection(list1, 0);

			Projection projection2 = new Projection(list1, 1);

			Projection projection3 = projection1.Add(projection2);

			List<double> result = new List<double>();

			result.Add(1.0);
			result.Add(4.3);
			result.Add(10.0);
			result.Add(12.7);
			result.Add(7.0);

			projection3.yearly_projections.Equals(result);


		}
	}
}
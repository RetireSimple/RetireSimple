using Amazon.Runtime;

using MongoDB.Driver.Core.Operations;

using RetireSimple.NewEngine.New_Engine.TaxModels;
using RetireSimple.NewEngine.New_Engine.TaxModels.IncomeTax;
using RetireSimple.NewEngine.New_Engine.TaxModels.IncomeTax.TaxBrackets;
using RetireSimple.NewEngine.New_Engine.TaxModels.TaxBrackets;
using RetireSimple.NewEngine.New_Engine.Users;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RetireSimple.NewTests {

	[TestClass]
	public class TestIncomeTax {

		[TestMethod]
		public void TestIncomeTaxSingle() {

			List<TaxCategory> taxCategories = new List<TaxCategory>();

			taxCategories.Add(TestIncomeTax.GenerateSingle());

			IncomeBrackets brackets = new IncomeBrackets(TestIncomeTax.GenerateRates(), taxCategories);

			User user = new User(new UserInfo(30, 65, 1000000, UserTaxStatus.SINGLE));

			user.AddTax(new IncomeTax(brackets, UserTaxStatus.SINGLE));

			double b1 = user.ApplyTax(1000);
			double b2 = user.ApplyTax(12000);
			double b3 = user.ApplyTax(45000);
			double b4 = user.ApplyTax(100000);
			double b5 = user.ApplyTax(200000);
			double b6 = user.ApplyTax(250000);
			Assert.AreEqual(1000 * (1 -.10), b1);
			Assert.AreEqual(12000 * (1 - .12), b2);
			Assert.AreEqual(45000 * (1 - .22), b3);
			Assert.AreEqual(100000 * (1 - .24), b4);
			Assert.AreEqual(200000 * (1 - .32), b5);
			Assert.AreEqual(250000 * (1 - .35), b6);


		}

		[TestMethod]
		public void TestIncomeTaxMarriedJointly() {

			List<TaxCategory> taxCategories = new List<TaxCategory>();

			taxCategories.Add(TestIncomeTax.GenerateMarriedJoint());

			IncomeBrackets brackets = new IncomeBrackets(TestIncomeTax.GenerateRates(), taxCategories);

			User user = new User(new UserInfo(30, 65, 1000000, UserTaxStatus.MARRIED_FILING_JOINTLY));

			user.AddTax(new IncomeTax(brackets, UserTaxStatus.MARRIED_FILING_JOINTLY));

			double b1 = user.ApplyTax(1000);
			double b2 = user.ApplyTax(25000);
			double b3 = user.ApplyTax(100000);
			double b4 = user.ApplyTax(200000);
			double b5 = user.ApplyTax(400000);
			double b6 = user.ApplyTax(550000);
			Assert.AreEqual(1000 * (1 - .10), b1);
			Assert.AreEqual(25000 * (1 - .12), b2);
			Assert.AreEqual(100000 * (1- .22), b3);
			Assert.AreEqual(200000 * (1 - .24), b4);
			Assert.AreEqual(400000 * (1 - .32), b5);
			Assert.AreEqual(550000 * (1 - .35), b6);

		}

		private static TaxCategory GenerateSingle() {
		
			List<TaxRange> taxRanges = new List<TaxRange>();

			taxRanges.Add(new TaxRange(0, 11000));
			taxRanges.Add(new TaxRange(11001, 44725));
			taxRanges.Add(new TaxRange(44726, 95375));
			taxRanges.Add(new TaxRange(95375, 182000));
			taxRanges.Add(new TaxRange(182101, 231250));
			taxRanges.Add(new TaxRange(231251, 578125));

			TaxCategory single = new TaxCategory(UserTaxStatus.SINGLE, taxRanges);

			return single;
		}


		private static TaxCategory GenerateMarriedJoint() {
	
			List<TaxRange> taxRanges = new List<TaxRange>();

			taxRanges.Add(new TaxRange(0, 22000));
			taxRanges.Add(new TaxRange(22001, 89450));
			taxRanges.Add(new TaxRange(89451, 190750));
			taxRanges.Add(new TaxRange(190751, 364200));
			taxRanges.Add(new TaxRange(364201, 462500));
			taxRanges.Add(new TaxRange(462501, 693750));

			TaxCategory marriedJoint = new TaxCategory(UserTaxStatus.MARRIED_FILING_JOINTLY, taxRanges);

			return marriedJoint;
		}

		public static List<double> GenerateRates() {
			List<double> rates = new List<double>();
			rates.Add(.10);
			rates.Add(.12);
			rates.Add(.22);
			rates.Add(.24);
			rates.Add(.32);
			rates.Add(.35);
			rates.Add(.37);

			return rates;
		}


		[TestMethod]
		public void TestingMultipleCats() {

			List<TaxCategory> taxCategories = new List<TaxCategory>();

			taxCategories.Add(TestIncomeTax.GenerateSingle());
			taxCategories.Add(TestIncomeTax.GenerateMarriedJoint());

			IncomeBrackets brackets = new IncomeBrackets(TestIncomeTax.GenerateRates(), taxCategories);

			User user = new User(new UserInfo(30, 65, 1000000, UserTaxStatus.MARRIED_FILING_JOINTLY));

			user.AddTax(new IncomeTax(brackets, UserTaxStatus.MARRIED_FILING_JOINTLY));

			Assert.AreEqual(250000 * (1 - .24)  , user.ApplyTax(250000));
		}


	}
}

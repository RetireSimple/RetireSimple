import {flattenApiInvestment} from '../data/ApiMapper';
import {Investment} from '../data/Interfaces';

describe('apiMapper', () => {
	describe('flattenApiInvestment', () => {
		test('should flatten an investment object - stock version', () => {
			const apiData: Investment = {
				investmentId: 2,
				investmentName: 'Test Investment 2',
				investmentType: 'StockInvestment',
				investmentData: {
					stockPrice: '250',
					stockQuantity: '25',
					stockTicker: 'TZST',
					stockPurchaseDate: '1/25/2023 4:04:08 PM',
					stockDividendPercent: '0.05',
					stockDividendDistributionInterval: 'Month',
					stockDividendDistributionMethod: 'Stock',
					stockDividendFirstPaymentDate: '1/1/2020',
				},
				analysisOptionsOverrides: {
					analysisLength: '60',
					simCount: '1000',
					randomVariableMu: '0.05',
					randomVariableSigma: '0.1',
					randomVariableScaleFactor: '1',
				},
				analysisType: 'MonteCarlo_LogNormalDist',
				lastAnalysis: null,
				portfolioId: 1,
			};

			const expected = {
				investmentId: 2,
				investmentName: 'Test Investment 2',
				investmentType: 'StockInvestment',
				stockPrice: '250',
				stockQuantity: '25',
				stockTicker: 'TZST',
				stockPurchaseDate: '1/25/2023 4:04:08 PM',
				stockDividendPercent: '0.05',
				stockDividendDistributionInterval: 'Month',
				stockDividendDistributionMethod: 'Stock',
				stockDividendFirstPaymentDate: '1/1/2020',
				analysis_analysisLength: '60',
				analysis_simCount: '1000',
				analysis_randomVariableMu: '0.05',
				analysis_randomVariableSigma: '0.1',
				analysis_randomVariableScaleFactor: '1',
				analysisType: 'MonteCarlo_LogNormalDist',
			};

			const result = flattenApiInvestment(apiData);

			expect(result).toStrictEqual(expected);
		});
	});
});
